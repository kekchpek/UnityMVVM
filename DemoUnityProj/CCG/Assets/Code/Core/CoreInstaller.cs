using CCG.Core.Camera;
using CCG.Core.Input;
using CCG.Core.Screen;
using CCG.Core.UI;
using CCG.Models.Hand.Model;
using CCG.Models.Hand.Service;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.View;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.CoolPopup;
using CCG.MVVM.HandController;
using CCG.MVVM.LoadingPopup;
using CCG.MVVM.MainMenu;
using CCG.MVVM.MainScreen.View;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.StatsChanger;
using CCG.Services.ImageLoaderService;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _3dRoot;
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private Transform _popupRoot;
        [SerializeField] private InputController _inputControllerPrefab;

        [SerializeField] private GameObject _mainViewPrefab;
        [SerializeField] private GameObject _handControllerPrefab;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _mainMenu3dPrefab;
        [SerializeField] private GameObject _mainMenuUiPrefab;
        [SerializeField] private GameObject _loadingPopupPrefab;
        [SerializeField] private GameObject _coolPopupPrefab;
        
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new []
            {
                (Main: ViewLayerIds.Main3d, _3dRoot),
                (Main: ViewLayerIds.MainUI, _uiRoot),
                (Main: ViewLayerIds.Popup, _popupRoot)
            });

            Container.FastBind<IHandMutableModel, IHandModel, HandModel>();
            Container.FastBindMono<IHandService, HandService>();
            Container.FastBind<ICameraMutableModel, ICameraModel, CameraModel>();
            Container.FastBind<ICameraService, CameraService>();
            Container.FastBind<IUiMutableModel, IUiModel, UiModel>();
            Container.FastBind<IImageModel, ImageModel>();
            Container.FastBind<IImageLoaderService, ImageLoaderService>();
            Container.FastBind<ICardFactory, CardFactory>();
            Container.FastBind<IUiService, UiService>();
            
            Container.GetViewModelsContainer().Bind<IScreenAdapter>().To<ScreenAdapter>().AsSingle();
            Container.GetViewModelsContainer().Bind<IInputController>().FromComponentInNewPrefab(_inputControllerPrefab).AsSingle();
            
            Container.InstallView<MainScreenView, IMainScreenViewModel, MainScreenViewModel>(ViewNames.MainScreen, _mainViewPrefab);
            Container.InstallView<StatsChangerView, IStatsChangerViewModel, StatsChangerViewModel>();
            Container.InstallView<HandControllerView, IHandControllerViewModel, HandControllerViewModel>(ViewNames.HandController, _handControllerPrefab);
            Container.InstallView<CardView, ICardViewModel, CardViewModel>(ViewNames.Card, _cardPrefab);
            Container.InstallView<MainMenuView3d, IMainMenuViewModel3d, MainMenuViewModel3d>(ViewNames.MainMenu3d, _mainMenu3dPrefab);
            Container.InstallView<MainMenuViewUi, IMainMenuViewModelUi, MainMenuViewModelUi>(ViewNames.MainMenuUi, _mainMenuUiPrefab);
            Container.InstallView<LoadingPopupView, IViewModel, ViewModel>(ViewNames.LoadingPopup, _loadingPopupPrefab);
            Container.InstallView<CoolPopupView, ICoolPopupViewModel, CoolPopupViewModel>(ViewNames.CoolPopup, _coolPopupPrefab);
            
            Container.ProvideAccessForViewLayer<ICameraService>();
        }
    }
}