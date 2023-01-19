using System;
using System.Collections.Generic;
using System.Linq;
using CCG.Services.ImageLoaderService;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.Promises.Factory;
using UnityEngine;

namespace CCG.Models.ImageModel
{
    public class ImageModel : IImageModel
    {
        private readonly IImageLoaderService _imageLoaderService;
        private readonly IPromiseFactory _promiseFactory;

        private int _pendingImagesCount;
        private IControllablePromise _initPromise;

        private readonly IDictionary<string, Texture2D> _images = new Dictionary<string, Texture2D>();

        public ImageModel(IImageLoaderService imageLoaderService,
            IPromiseFactory promiseFactory)
        {
            _imageLoaderService = imageLoaderService;
            _promiseFactory = promiseFactory;
        }

        public IPromise LoadImages()
        {
            _pendingImagesCount = Config.ConfigData.ImageInitBuffer;
            _initPromise = _promiseFactory.CreatePromise();
            for (var i = 0; i < Config.ConfigData.ImageInitBuffer; i++)
            {
                _imageLoaderService.LoadRandomImage()
                    .OnSuccess(image =>
                    {
                        AddImage(image);
                        CheckIsInitialized();
                    })
                    .OnFail(FailInitialization);
            }

            return _initPromise;
        }
        
        public Texture2D GetImage(string imageId)
        {
            return _images[imageId];
        }

        public string[] GetAllImageIds()
        {
            return _images.Keys.ToArray();
        }

        private void AddImage(Texture2D image)
        {
            _images.Add(Guid.NewGuid().ToString(), image);
        }

        private void CheckIsInitialized()
        {
            Debug.Log(_pendingImagesCount);
            if (--_pendingImagesCount == 0)
            {
                _initPromise.Success();
            }
        }

        private void FailInitialization(Exception e)
        {
            _pendingImagesCount = default;
            _images.Clear();
            _initPromise.Fail(new InvalidOperationException("Initialization failed!", e));
        }
    }
}