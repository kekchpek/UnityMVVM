using System;
using System.Collections.Generic;
using AsyncReactAwait.Promises;
using CCG.Services.Startup;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityMVVM.DI;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.Tests.Editor.Core
{
    public class TestApplication
    {
        private readonly DiContainer _appContainer;

        /// <summary>
        /// Creates a test application environment.
        /// </summary>
        /// <param name="decoratingMap">Entities to rebind.</param>
        /// <param name="rebindMap">Entities to decorate.</param>
        /// <returns>Entry point for test application environment.</returns>
        public static TestApplication Create(
            IReadOnlyCollection<(Type type, Func<object, object> decorator)> decoratingMap = null,
            IReadOnlyCollection<(Type type, object instance)> rebindMap = null)
        {
            var container = new DiContainer();
            container.Install<TestInstaller>();
            if (rebindMap != null)
            {
                foreach (var rebind in rebindMap)
                {
                    RebindContainerDependency(container, rebind.type, rebind.instance);
                    RebindContainerDependency(container.GetViewModelsContainer(), rebind.type, rebind.instance);
                    RebindContainerDependency(container.GetViewsContainer(), rebind.type, rebind.instance);
                }
            }
            if (decoratingMap != null)
            {
                foreach (var decoration in decoratingMap)
                {
                    DecorateContainerDependency(container, decoration.type, decoration.decorator);
                    DecorateContainerDependency(container.GetViewModelsContainer(), decoration.type, decoration.decorator);
                    DecorateContainerDependency(container.GetViewsContainer(), decoration.type, decoration.decorator);
                }
            }
            return new TestApplication(container);
        }

        private TestApplication(DiContainer appContainer)
        {
            _appContainer = appContainer;
        }

        public IPromise Start()
        {
            return _appContainer.Resolve<IStartupService>().Startup();
        }

        public T GetViewModel<T>(string viewLayer) where T : IViewModel
        {
            return (T)_appContainer.Resolve<IViewManager>().GetView(viewLayer);
        }
        
        public T GetViewModelDependency<T>()
        {
            return _appContainer.GetViewModelsContainer().Resolve<T>();
        }

        public void AssertViewOpened<T>(string layerName)
        {
            Assert.IsTrue(_appContainer.Resolve<IViewManager>().GetView(layerName) is T, 
                $"Type of view opened on layer {layerName} is not {typeof(T).Name}");
        }
        
        public void AssertViewOpened(string layerName, string viewName)
        {
            Assert.AreEqual(viewName, _appContainer.Resolve<IViewManager>().GetViewName(layerName));
        }
        
        public void AssertNoView(string layerName)
        {
            Assert.AreEqual(null, _appContainer.Resolve<IViewManager>().GetViewName(layerName));
        }

        private static void RebindContainerDependency(
            [NotNull] DiContainer container, 
            [NotNull] Type dependencyType,
            [NotNull] object newDependency)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (dependencyType == null) throw new ArgumentNullException(nameof(dependencyType));
            if (newDependency == null) throw new ArgumentNullException(nameof(newDependency));
            if (container.HasBinding(dependencyType))
            {
                container.Rebind(dependencyType).FromInstance(newDependency);
            }
        }
        
        private static void DecorateContainerDependency(
            [NotNull] DiContainer container, 
            [NotNull] Type dependencyType, 
            [NotNull] Func<object, object> decorator)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (dependencyType == null) throw new ArgumentNullException(nameof(dependencyType));
            if (decorator == null) throw new ArgumentNullException(nameof(decorator));
            if (container.HasBinding(dependencyType))
            {
                var decoratable = container.Resolve(dependencyType);
                container.Rebind(dependencyType).FromInstance(decorator(decoratable));
            }
        }
    }
}