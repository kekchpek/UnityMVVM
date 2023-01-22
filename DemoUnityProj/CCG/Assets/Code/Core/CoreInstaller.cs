using CCG.Core.Camera;
using CCG.Core.Input;
using CCG.Core.Screen;
using CCG.Core.UI;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.View;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.Hand.Model;
using CCG.MVVM.Hand.Service;
using CCG.MVVM.HandController;
using CCG.MVVM.MainMenu;
using CCG.MVVM.MainScreen.View;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.StatsChanger;
using CCG.Services.ImageLoaderService;
using UnityAuxiliaryTools.UnityExecutor;
using UnityEngine;
using UnityMVVM.DI;
using Zenject;

namespace CCG.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private Transform _3dRoot;
        [SerializeField] private InputController _inputControllerPrefab;

        [SerializeField] private GameObject _mainViewPrefab;
        [SerializeField] private GameObject _statsChangesPrefab;
        [SerializeField] private GameObject _handControllerPrefab;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private GameObject _mainMenu3dPrefab;
        [SerializeField] private GameObject _mainMenuUiPrefab;
        
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new []
            {
                (Main: ViewLayerIds.Main3d, _3dRoot),
                (Main: ViewLayerIds.MainUI, _uiRoot)
            });

            Container.FastBind<IHandMutableModel, IHandModel, HandModel>();
            Container.FastBind<IHandService, HandService>();
            Container.FastBind<ICameraMutableModel, ICameraModel, CameraModel>();
            Container.FastBind<ICameraService, CameraService>();
            Container.FastBind<IUiMutableModel, IUiModel, UiModel>();
            Container.FastBind<IImageModel, ImageModel>();
            Container.FastBind<IImageLoaderService, ImageLoaderService>();
            Container.FastBind<ICardFactory, CardFactory>();
            Container.FastBind<IUiService, UiService>();
            Container.Bind<IUnityExecutor>().To<UnityExecutor>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.GetViewModelsContainer().Bind<IScreenAdapter>().To<ScreenAdapter>().AsSingle();
            Container.GetViewModelsContainer().Bind<IInputController>().FromComponentInNewPrefab(_inputControllerPrefab).AsSingle();
            
            Container.InstallView<MainScreenView, IMainScreenViewModel, MainScreenViewModel>(_mainViewPrefab);
            Container.InstallView<StatsChangerView, IStatsChangerViewModel, StatsChangerViewModel>(_statsChangesPrefab);
            Container.InstallView<HandControllerView, IHandControllerViewModel, HandControllerViewModel>(_handControllerPrefab);
            Container.InstallView<CardView, ICardViewModel, CardViewModel>(_cardPrefab);
            Container.InstallView<MainMenuView3d, IMainMenuViewModel3d, MainMenuViewModel3d>(_mainMenu3dPrefab);
            Container.InstallView<MainMenuViewUi, IMainMenuViewModelUi, MainMenuViewModelUi>(_mainMenuUiPrefab);
            
            Container.ProvideAccessForViewLayer<ICameraService>();
        }
    }
}