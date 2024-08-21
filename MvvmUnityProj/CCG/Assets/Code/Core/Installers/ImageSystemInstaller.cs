using CCG.Models.ImageModel;
using CCG.Services.ImageLoaderService;
using UnityMVVM.DI;
using Zenject;

namespace CCG.Core.Installers
{
    public class ImageSystemInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.FastBind<ImageModel>(new []{typeof(IImageService), typeof(IImageModel)});
            Container.FastBind<IImageLoaderService, ImageLoaderService>();
        }
    }
}