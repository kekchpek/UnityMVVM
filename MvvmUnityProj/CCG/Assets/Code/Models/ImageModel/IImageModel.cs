using AsyncReactAwait.Promises;
using UnityEngine;

namespace CCG.Models.ImageModel
{
    public interface IImageModel
    {
        Texture2D GetImage(string imageId);
        string[] GetAllImageIds();
    }
}