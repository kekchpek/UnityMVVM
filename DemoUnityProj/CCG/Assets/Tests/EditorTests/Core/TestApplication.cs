using AsyncReactAwait.Promises;
using CCG.Services.Startup;
using NUnit.Framework;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.Tests.Editor.Core
{
    public class TestApplication
    {
        private readonly DiContainer _appContainer;

        public static TestApplication Create()
        {
            var container = new DiContainer();
            container.Install<TestInstaller>();
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
    }
}