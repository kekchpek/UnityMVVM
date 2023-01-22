using System;
using CCG.Core;
using CCG.Models.Hand.Model;
using CCG.Models.Hand.Service;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.ViewModel;
using UnityEngine;
using UnityMVVM.ViewManager;
using Zenject;
using Random = UnityEngine.Random;

namespace CCG.MVVM.MainScreen.ViewModel
{
    public class MainScreenViewModel : UnityMVVM.ViewModelCore.ViewModel, IMainScreenViewModel, IInitializable
    {
        private readonly IImageModel _imageModel;
        private readonly IHandService _handService;
        private readonly IHandModel _handModel;
        private readonly IViewManager _viewManager;

        public MainScreenViewModel(IImageModel imageModel, IHandService handService,
            IHandModel handModel, IViewManager viewManager)
        {
            _imageModel = imageModel;
            _handService = handService;
            _handModel = handModel;
            _viewManager = viewManager;
        }
        
        public void Initialize()
        {
            CreateSubView(ViewNames.HandController);
            CreateSubView(ViewNames.StatsChanger);
            _handModel.CardAdded += OnCardAdded;
            IntiGame();
        }

        private void OnCardAdded(ICardModel card)
        {
            CreateSubView(ViewNames.Card, new CardPayload(card));
        }
        
        private void IntiGame()
        {
            OpenView(ViewLayerIds.Popup, ViewNames.LoadingPopup);
            _imageModel.LoadImages().OnSuccess(() =>
            {
                var cardsCount = Random.Range(4, 7);
                for (var i = 0; i < cardsCount; i++)
                {
                    _handService.AddRandomCardToHand();
                }
                _viewManager.Close(ViewLayerIds.Popup);
            }).OnFail(e =>
            {
                Debug.LogError(e.Message);
                // perpetual retrying
                IntiGame();
            });
        }

        public void OnMainMenuButtonClicked()
        {
            OpenView(ViewLayerIds.Main3d, ViewNames.MainMenu3d);
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _handService.ClearHand();
            _handModel.CardAdded -= OnCardAdded;
        }
    }
}