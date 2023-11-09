using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AsyncReactAwait.Promises;
using NSubstitute.Core;
using NSubstitute.Routing.AutoValues;

namespace CCG.Tests.Editor.Core.SubstituteValueProviders
{
    public class PromiseValueProviderFactory : IAutoValueProvidersFactory
    {
        private readonly IAutoValueProvidersFactory _original;

        private class PromiseValueProvider : IAutoValueProvider
        {
            private readonly IReadOnlyCollection<IAutoValueProvider> _providers;

            private readonly Dictionary<Type, object> _completedPromises = new();

            public PromiseValueProvider(IReadOnlyCollection<IAutoValueProvider> providers)
            {
                _providers = providers;
            }

            public bool CanProvideValueFor(Type type)
            {
                return type == typeof(IPromise) ||
                       type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IPromise<>);
            }

            public object GetValue(Type type)
            {
                if (type == typeof(IPromise))
                    return PromiseTool.GetSuccessful();
                if (type.GetGenericTypeDefinition() == typeof(IPromise<>))
                    return GetSuccessful(type.GenericTypeArguments.Single());
                throw new InvalidOperationException($"Can not create promise for type {type.Name}");
            }

            private object GetSuccessful(Type t)
            {
                if (_completedPromises.TryGetValue(t, out var p))
                    return p;
                var promiseTool = typeof(PromiseTool);
                var getMethod = promiseTool.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Single(x => x.Name == nameof(PromiseTool.GetSuccessful) && x.IsGenericMethod);
                var resultSubstituteProvider = _providers.FirstOrDefault(x => x.CanProvideValueFor(t));
                Expression resultExpression = resultSubstituteProvider == null
                    ? Expression.Default(t)
                    : Expression.Constant(resultSubstituteProvider.GetValue(t));
                var completedPromise =
                    Expression.Lambda(Expression.Call(
                        getMethod.MakeGenericMethod(t),
                        resultExpression)).Compile().DynamicInvoke();
                _completedPromises.Add(t, completedPromise);
                return completedPromise;
            }
        }

        public PromiseValueProviderFactory(IAutoValueProvidersFactory original)
        {
            _original = original;
        }

        public IReadOnlyCollection<IAutoValueProvider> CreateProviders(ISubstituteFactory substituteFactory)
        {
            var originalProviders = _original.CreateProviders(substituteFactory);
            return new[] { new PromiseValueProvider(originalProviders) }.Concat(originalProviders).ToArray();
        }
    }
}