using CCG.Services.ImageLoaderService;
using UnityEngine;
using Zenject;

namespace CCG.Services
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
    public class ServicesInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IImageLoaderService>().To<ImageLoaderService.ImageLoaderService>().AsSingle();
        }
    }
}