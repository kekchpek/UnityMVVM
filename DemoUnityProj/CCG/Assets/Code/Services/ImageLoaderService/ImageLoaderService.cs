using System;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityEngine.Networking;

namespace CCG.Services.ImageLoaderService
{
    public class ImageLoaderService : IImageLoaderService
    {
        private const string RandomImageURL = "https://picsum.photos/200";

        public IPromise<Texture2D> LoadRandomImage()
        {
            var request = UnityWebRequestTexture.GetTexture(RandomImageURL);
            var promise = new ControllablePromise<Texture2D>();

            void Completed(AsyncOperation op)
            {
                op.completed -= Completed;
                if (request.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(request.error);
                    promise.Fail(new Exception(request.error));
                }
                else {
                    promise.Success(((DownloadHandlerTexture)request.downloadHandler).texture);
                }
            }
            request.SendWebRequest().completed += Completed;
            return promise;
        }
    }
}