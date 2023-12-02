using CCG.Core.Camera;
using CCG.Core.Installers;
using CCG.Core.Screen;
using CCG.Models.Hand.Model;
using CCG.Models.Hand.Service;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.View;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.CoolPopup;
using CCG.MVVM.HandController;
using CCG.MVVM.LoadingPopup;
using CCG.MVVM.MainMenu;
using CCG.MVVM.MainScreen.View;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.MainScreen3d;
using CCG.MVVM.PlayButton;
using CCG.MVVM.StatsChanger;
using CCG.MVVM.TimeCounter;
using CCG.Services.Game;
using CCG.Services.Startup;
using SurvivedWarrior.MVVM.Models.Time;
using UnityEngine;
using UnityMVVM.DI;
using UnityMVVM.ViewModelCore;
using Zenject;

namespace CCG.Core
{
    public class CoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.InstallPoolableView<MainScreenView, IMainScreenViewModel, MainScreenViewModel>(ViewNames.MainScreen,
                () => Resources.Load<GameObject>("Prefabs/Views/MainScreenView"));
            Container.InstallView<MainScreen3dView, IViewModel, ViewModel>(ViewNames.MainScreen3d,
                () => Resources.Load<GameObject>("Prefabs/Views/MainScreen3dView"));
            Container.InstallView<StatsChangerView, IStatsChangerViewModel, StatsChangerViewModel>();
            Container.InstallView<PlayButtonView, IPlayButtonViewModel, PlayButtonViewModel>();
            Container.InstallView<HandControllerView, IHandControllerViewModel, HandControllerViewModel>(ViewNames.HandController, 
                () => Resources.Load<GameObject>("Prefabs/Views/HandController"));
            Container.InstallPoolableView<CardView, ICardViewModel, CardViewModel>(ViewNames.Card, 
                () => Resources.Load<GameObject>("Prefabs/Views/CardView"));
            Container.InstallView<MainMenuView3d, IMainMenuViewModel3d, MainMenuViewModel3d>(ViewNames.MainMenu3d, 
                () => Resources.Load<GameObject>("Prefabs/Views/MainMenu3d/MainMenu3dScene"));
            Container.InstallView<MainMenuViewUi, IMainMenuViewModelUi, MainMenuViewModelUi>(ViewNames.MainMenuUi, 
                () => Resources.Load<GameObject>("Prefabs/Views/MainMenuUi/MainMenuUi"));
            Container.InstallView<LoadingPopupView, IViewModel, ViewModel>(ViewNames.LoadingPopup, 
                () => Resources.Load<GameObject>("Prefabs/Views/LoadingPopup"));
            Container.InstallView<CoolPopupView, ICoolPopupViewModel, CoolPopupViewModel>(ViewNames.CoolPopup, 
                () => Resources.Load<GameObject>("Prefabs/Views/CoolPopup/CoolPopup"));
            Container.InstallView<TimeCounterView, ITimeCounterViewModel, TimeCounterViewModel>();
            
            Container.Install<ImageSystemInstaller>();
            
            Container.FastBind<IStartupService, StartupService>();
            
            Container.FastBindMono<ITimeManager, TimeManager>();
            
            Container.GetViewsContainer()
                .Bind(typeof(ICameraMutableModel), typeof(ICameraModel))
                .To<CameraModel>().AsSingle();
            Container.GetViewsContainer()
                .Bind<ICameraService>().To<CameraService>().AsSingle();
            Container.GetViewsContainer().Bind<IScreenAdapter>().To<ScreenAdapter>().AsSingle();
            
            Container.FastBind<IGameService, GameService>();
            
            Container.FastBind<IHandMutableModel, IHandModel, HandModel>();
            Container.FastBindMono<IHandService, HandService>();
            Container.FastBind<ICardFactory, CardFactory>();
        }
    }
}