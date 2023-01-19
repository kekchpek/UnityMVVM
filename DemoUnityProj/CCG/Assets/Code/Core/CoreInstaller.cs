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

            var modelLayerContainer = Container.CreateSubContainer();
            modelLayerContainer.Bind<IHandMutableModel>().To<HandModel>().AsSingle();
            modelLayerContainer.Bind<IHandService>().To<HandService>().AsSingle();
            modelLayerContainer.Bind<ICameraMutableModel>().To<CameraModel>().AsSingle();
            modelLayerContainer.Bind<ICameraService>().To<CameraService>().AsSingle();
            
            Container.Bind<ICardFactory>().To<CardFactory>().AsSingle();
            Container.Bind<IScreenAdapter>().To<ScreenAdapter>().AsSingle();
            Container.Bind<IInputController>().FromComponentInNewPrefab(_inputControllerPrefab).AsSingle();
            Container.Bind<IUnityExecutor>().To<UnityExecutor>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<IPromiseFactory>().To<PromiseFactory>().AsSingle();
            Container.Bind<IImageModel>().To<ImageModel>().AsSingle();
            Container.Bind<IImageLoaderService>().To<ImageLoaderService>().AsSingle();
            Container.Bind<ICameraService>().FromMethod(x => modelLayerContainer.Resolve<ICameraService>());
            Container.Bind<ICameraModel>().FromMethod(x => modelLayerContainer.Resolve<ICameraMutableModel>());
            Container.Bind<IHandModel>().FromMethod(x => modelLayerContainer.Resolve<IHandMutableModel>());
            Container.Bind<IHandService>().FromMethod(x => modelLayerContainer.Resolve<IHandService>());

            var mvvmContainer = new MvvmSubContainer(Container, new [] {(ViewLayerIds.Main, _viewRoot)});
            mvvmContainer
                .InstallFactoryFor<MainScreenView, IMainScreenViewModel, MainScreenViewModel>(_mainViewPrefab);
            mvvmContainer
                .InstallFactoryFor<StatsChangerView, IStatsChangerViewModel, StatsChangerViewModel>(_statsChangesPrefab);
            mvvmContainer.
                InstallFactoryFor<HandControllerView, IHandControllerViewModel, HandControllerViewModel>(_handControllerPrefab);
            mvvmContainer
                .InstallFactoryFor<CardView, ICardViewModel, CardViewModel>(_cardPrefab);
        }
    }
}