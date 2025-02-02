using System;
using AsyncReactAwait.Promises;
using CCG.Core.Camera;
using UnityEngine;
using UnityMVVM;
using Zenject;

namespace CCG.MVVM.MainMenu
{
    public class MainMenuView3d : ViewBehaviour<IMainMenuViewModel3d>
    {

        [SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;

        private ICameraService _cameraService;

        private IControllablePromise _exitPromise;
        private static readonly int ExitTrigger = Animator.StringToHash("Exit");

        [Inject]
        public void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        protected override void Awake()
        {
            base.Awake();
            _cameraService.UseCamera(_camera);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel!.State, OnStateChanged);
        }

        // required for animation
        private void Animation_StateChanged()
        {
            ViewModel!.OnStateChangeCompleted();
        }
        
        private void Animation_Exit()
        {
            _exitPromise.Success();
        }


        private void OnStateChanged(MainMenuState state)
        {
            string trigger;
            switch (state)
            {
                case MainMenuState.None:
                    trigger = "MoveBack";
                    break;
                case MainMenuState.Cube:
                    trigger = "MoveToCube";
                    break;
                case MainMenuState.Cylinder:
                    trigger = "MoveToCylinder";
                    break;
                case MainMenuState.Sphere:
                    trigger = "MoveToSphere";
                    break;
                case MainMenuState.Capsule:
                    trigger = "MoveToCapsule";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            _animator.SetTrigger(trigger);
        }

        protected override IPromise Close()
        {
            _animator.SetTrigger(ExitTrigger);
            _exitPromise = new ControllablePromise();
            return _exitPromise;
        }
    }
}
