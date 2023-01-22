using System;
using System.Collections;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.UnityExecutor;
using UnityEngine;
using UnityEngine.Networking;

namespace CCG.Services.ImageLoaderService
{
    public class ImageLoaderService : IImageLoaderService
    {
        private readonly IUnityExecutor _unityExecutor;

        private const string RandomImageURL = "https://picsum.photos/200";

        public ImageLoaderService(
            IUnityExecutor unityExecutor)
        {
            _unityExecutor = unityExecutor;
        }
        
        public IPromise<Texture2D> LoadRandomImage()
        {
            IControllablePromise<Texture2D> promise = new ControllablePromise<Texture2D>();
            _unityExecutor.ExcuteCoroutine(DownloadImageCoroutine(promise));
            return promise;
        }

        private IEnumerator DownloadImageCoroutine(IControllablePromise<Texture2D> downloadPromise)
        {
            var request = UnityWebRequestTexture.GetTexture(RandomImageURL);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError(request.error);
                downloadPromise.Fail(new Exception(request.error));
            }
            else {
                downloadPromise.Success(((DownloadHandlerTexture)request.downloadHandler).texture);
            }
        }
    }
}