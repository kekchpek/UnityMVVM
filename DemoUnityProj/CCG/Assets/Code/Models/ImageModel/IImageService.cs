using AsyncReactAwait.Promises;

namespace CCG.Models.ImageModel
{
    public interface IImageService
    {
        IPromise LoadImages();
    }
}