using System;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.Hand.Model;
using CCG.MVVM.Hand.Service;
using CCG.MVVM.HandController;
using CCG.MVVM.StatsChanger;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CCG.MVVM.MainScreen.ViewModel
{
    public class MainScreenViewModel : UnityMVVM.ViewModelCore.ViewModel, IMainScreenViewModel, IInitializable
    {
        private readonly IImageModel _imageModel;
        private readonly IHandService _handService;
        private readonly IHandModel _handModel;

        public event Action LoadingCompleted;

        public MainScreenViewModel(IImageModel imageModel, IHandService handService,
            IHandModel handModel)
        {
            _imageModel = imageModel;
            _handService = handService;
            _handModel = handModel;
        }
        
        public void Initialize()
        {
            CreateSubView<IHandControllerViewModel>();
            CreateSubView<IStatsChangerViewModel>();
            _handModel.CardAdded += OnCardAdded;
            IntiGame();
        }

        private void OnCardAdded(ICardModel card)
        {
            CreateSubView<ICardViewModel>(new CardPayload(card));
        }
        
        private void IntiGame()
        {
            _imageModel.LoadImages().OnSuccess(() =>
            {
                var cardsCount = Random.Range(4, 7);
                for (var i = 0; i < cardsCount; i++)
                {
                    _handService.AddRandomCardToHand();
                }
                LoadingCompleted?.Invoke();
            }).OnFail(e =>
            {
                Debug.LogError(e.Message);
                // perpetual retrying
                IntiGame();
            });
        }
    }
}