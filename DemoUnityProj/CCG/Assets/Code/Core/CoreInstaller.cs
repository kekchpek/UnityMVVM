using CCG.Core.Camera;
using CCG.Core.Input;
using CCG.Core.Screen;
using CCG.Models.ImageModel;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.View;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.Hand.Model;
using CCG.MVVM.Hand.Service;
using CCG.MVVM.HandController;
using CCG.MVVM.MainScreen.View;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.StatsChanger;
using CCG.Services.ImageLoaderService;
using UnityAuxiliaryTools.Promises.Factory;
using UnityAuxiliaryTools.UnityExecutor;
using UnityEngine;
using UnityMVVM.DI;
using Zenject;

namespace CCG.Core
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _viewRoot;
        [SerializeField] private InputController _inputControllerPrefab;

        [SerializeField] private GameObject _mainViewPrefab;
        [SerializeField] private GameObject _statsChangesPrefab;
        [SerializeField] private GameObject _handControllerPrefab;
        [SerializeField] private GameObject _cardPrefab;
        
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new [] {(ViewLayerIds.Main, _viewRoot)});

            Container.FastBind<IHandMutableModel, IHandModel, HandModel>();
            Container.FastBind<IHandService, HandService>();
            Container.FastBind<ICameraMutableModel, ICameraModel, CameraModel>();
            Container.FastBind<ICameraService, CameraService>();
            Container.FastBind<IImageModel, ImageModel>();
            Container.FastBind<IImageLoaderService, ImageLoaderService>();
            Container.FastBind<ICardFactory, CardFactory>();
            Container.FastBind<IPromiseFactory, PromiseFactory>();
            Container.Bind<IUnityExecutor>().To<UnityExecutor>().FromNewComponentOnNewGameObject().AsSingle();
            
            Container.GetViewModelsContainer().Bind<IScreenAdapter>().To<ScreenAdapter>().AsSingle();
            Container.GetViewModelsContainer().Bind<IInputController>().FromComponentInNewPrefab(_inputControllerPrefab).AsSingle();
            
            Container.InstallView<MainScreenView, IMainScreenViewModel, MainScreenViewModel>(_mainViewPrefab);
            Container.InstallView<StatsChangerView, IStatsChangerViewModel, StatsChangerViewModel>(_statsChangesPrefab);
            Container.InstallView<HandControllerView, IHandControllerViewModel, HandControllerViewModel>(_handControllerPrefab);
            Container.InstallView<CardView, ICardViewModel, CardViewModel>(_cardPrefab);
        }
    }
}