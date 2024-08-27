using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute.Core;
using NSubstitute.Routing.AutoValues;
using UnityEngine;

namespace CCG.Tests.Editor.Core.SubstituteValueProviders
{
    public class UnityObjectsValueProviderFactory : IAutoValueProvidersFactory
    {
        private readonly IAutoValueProvidersFactory _original;

        private class UnityObjectsValueProvider : IAutoValueProvider
        {
            private static readonly Dictionary<Type, object> ProvidingTypes = new()
            {
                { typeof(Texture2D), Texture2D.blackTexture }
            };
            
            private readonly IReadOnlyCollection<IAutoValueProvider> _providers;

            private readonly Dictionary<Type, object> _completedPromises = new();

            public UnityObjectsValueProvider(IReadOnlyCollection<IAutoValueProvider> providers)
            {
                _providers = providers;
            }

            public bool CanProvideValueFor(Type type)
            {
                return ProvidingTypes.ContainsKey(type);
            }

            public object GetValue(Type type)
            {
                if (ProvidingTypes.TryGetValue(type, out var value))
                {
                    return value;
                }
                throw new InvalidOperationException($"Can not create value for type {type.Name}");
            }
        }

        public UnityObjectsValueProviderFactory(IAutoValueProvidersFactory original)
        {
            _original = original;
        }

        public IReadOnlyCollection<IAutoValueProvider> CreateProviders(ISubstituteFactory substituteFactory)
        {
            var originalProviders = _original.CreateProviders(substituteFactory);
            return new[] { new UnityObjectsValueProvider(originalProviders) }.Concat(originalProviders).ToArray();
        }
    }
}