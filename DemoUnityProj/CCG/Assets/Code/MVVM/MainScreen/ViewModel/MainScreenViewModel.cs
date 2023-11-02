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
        private readonly IImageService _imageService;
        private readonly IHandService _handService;
        private readonly IHandModel _handModel;
        private readonly IViewManager _viewManager;

        private Transform _cardsContainer;

        public MainScreenViewModel(IImageService imageService, IHandService handService,
            IHandModel handModel, IViewManager viewManager)
        {
            _imageService = imageService;
            _handService = handService;
            _handModel = handModel;
            _viewManager = viewManager;
        }
        
        public void Initialize()
        {
            _handModel.CardAdded += OnCardAdded;
            OpenView(ViewLayerIds.Popup, ViewNames.LoadingPopup);
            IntiGame();
        }

        private void OnCardAdded(ICardModel card)
        {
            CreateSubView(ViewNames.Card, _cardsContainer, new CardPayload(card));
        }
        
        private void IntiGame()
        {
            _imageService.LoadImages().OnSuccess(() =>
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
                OpenView(ViewLayerIds.Main3d, ViewNames.MainMenu3d);
            });
        }

        public void OnMainMenuButtonClicked()
        {
            OpenView(ViewLayerIds.Main3d, ViewNames.MainMenu3d);
        }

        public void SetCardsContainer(Transform container)
        {
            _cardsContainer = container;
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _handService.ClearHand();
            _handModel.CardAdded -= OnCardAdded;
        }
    }
}