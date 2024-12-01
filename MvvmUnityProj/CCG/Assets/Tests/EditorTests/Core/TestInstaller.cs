using CCG.Core;
using CCG.Services.ImageLoaderService;
using NSubstitute;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.DI.Config;
using Zenject;

namespace CCG.Tests.Editor.Core
{
    public class TestInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new[]
                {
                    (ViewLayerIds.Main3d, CreateLayerMock()),
                    (ViewLayerIds.MainUI, CreateLayerMock()),
                    (ViewLayerIds.Popup, CreateLayerMock())
                },
                new MvvmContainerConfiguration
                {
                    viewFactory = new TestViewFactory()
                });
            Container.Install<CoreInstaller>();
            
            // Rebind low level dependencies
            Container.Rebind<IImageLoaderService>().FromInstance(Substitute.For<IImageLoaderService>());
        }

        private static Transform CreateLayerMock()
        {
            return new GameObject().transform;
        }
    }
}