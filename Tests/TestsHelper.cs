using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zenject;

namespace Tests
{
    internal static class TestsHelper
    {

        /// <summary>
        /// Creates a container with an object of type <typeparamref name="T"/> and all its mocked dependencies inside.
        /// </summary>
        /// <param name="createdObj">Created object of type <typeparamref name="T"/> inside container.</param>
        /// <typeparam name="T">The type of object that should be created, and dependencies of which should be mocked.</typeparam>
        /// <returns>Returns a container with created object and all its mocked dependencies.</returns>
        /// <exception cref="InvalidOperationException">The type <typeparamref name="T"/> has several constructors or have no one.</exception>
        public static DiContainer CreateContainerFor<T>(out T createdObj, bool useRealInstantiator = false)
        {
            return CreateContainerFor(new Dictionary<Type, object>(), out createdObj, useRealInstantiator);
        }

        /// <inheritdoc cref="CreateContainerFor{T}(out T)"/>
        /// <param name="explicitDependencies">The explicitly specified dependencies, that will be passed to the constructor.
        /// You can specify here own stubs or dependencies, that can't be mocked automatically with NSubstitute.</param>
        /// <exception cref="ArgumentException">The type of the dependency in <paramref name="explicitDependencies"/> doesn't corresponds the provided object type.</exception>
        public static DiContainer CreateContainerFor<T>(IReadOnlyDictionary<Type, object> explicitDependencies, out T createdObj, bool useRealInstantiator = false)
        {
            var type = typeof(T);
            var container = new DiContainer();
            var constructorInfos = type.GetConstructors();
            if (constructorInfos.Length != 1)
            {
                throw new InvalidOperationException(
                    "Can not determine what constructor should be used for mocks creating");
            }
            foreach (var argType in constructorInfos.First().GetParameters().Select(x => x.ParameterType))
            {
                // IInstantiator should be rebind because it is bound by default
                if (explicitDependencies.TryGetValue(argType, out var dependency))
                {
                    if (!argType.IsInstanceOfType(dependency))
                    {
                        throw new ArgumentException(
                            $"Argument of type {dependency.GetType()} is not assignable to {argType} argument");
                    }
                    container.Bind(argType)
                        .FromInstance(dependency)
                        .AsSingle();
                }
                else
                {
                    if (argType == typeof(IInstantiator))
                    {
                        if (!useRealInstantiator)
                        {
                            container.Rebind(argType)
                                .FromInstance(Substitute.For(new[] { argType }, Array.Empty<object>()))
                                .AsSingle();
                        }
                    }
                    else
                    {
                        container.Bind(argType)
                            .FromInstance(Substitute.For(new[] { argType }, Array.Empty<object>()))
                            .AsSingle();
                    }
                }
            }
            container.Bind<T>().ToSelf().AsSingle();
            createdObj = container.Resolve<T>();
            return container;
        }

        public static DiContainer CreateContainerForPartsOf<T>(out T createdObject)
            where T : class
        {
            var type = typeof(T);
            var container = new DiContainer();
            var constructorInfos = type.GetConstructors();
            if (constructorInfos.Length != 1)
            {
                throw new InvalidOperationException(
                    "Can not determine what constructor should be used for mocks creating");
            }

            var ctorTypes = constructorInfos.First().GetParameters().Select(x => x.ParameterType).ToArray();
            foreach (var argType in ctorTypes)
            {
                container.Bind(argType)
                    .FromInstance(Substitute.For(new[] { argType }, Array.Empty<object>()))
                    .AsSingle();
            }

            var ctorArgs = ctorTypes.Select(argType => container.Resolve(argType)).ToArray();
            createdObject = Substitute.ForPartsOf<T>(ctorArgs);
            container.Bind<T>()
                .FromInstance(createdObject)
                .AsSingle();

            return container;
        }
        /// <summary>
        /// Ads a dependencies of an object of type <typeparamref name="T"/> to existing container.
        /// </summary>
        /// <param name="container">Existing container.</param>
        /// <typeparam name="T">The type of object which dependencies should be added to the container.</typeparam>
        /// <exception cref="InvalidOperationException">The type <typeparamref name="T"/> has several constructors or have no one.</exception>
        public static void AddDependenciesToContainer<T>(this DiContainer container)
        {
            var type = typeof(T);
            var constructorInfos = type.GetConstructors();
            if (constructorInfos.Length != 1)
            {
                throw new InvalidOperationException(
                    "Can not determine what constructor should be used for mocks creating");
            }
            foreach (var argType in constructorInfos.First().GetParameters().Select(x => x.ParameterType))
            {
                if (argType == typeof(IInstantiator) || container.HasBinding(argType))
                {
                    continue;
                }

                container.Bind(argType)
                    .FromInstance(Substitute.For(new[] { argType }, Array.Empty<object>()))
                    .AsSingle();
            }
        }
    }
}
