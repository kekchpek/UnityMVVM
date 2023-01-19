using UnityAuxiliaryTools.Promises;
using UnityEngine;

namespace CCG.Services.ImageLoaderService
{
    public interface IImageLoaderService
    {
        IPromise<Texture2D> LoadRandomImage();
    }
}