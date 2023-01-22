using System;
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

        [Inject]
        public void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        private void Awake()
        {
            _cameraService.UseCamera(_camera);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.State.Bind(OnStateChanged);
        }

        // required for animation
        private void Animation_StateChanged()
        {
            ViewModel.OnStateChangeCompleted();
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

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            ViewModel.State.Unbind(OnStateChanged);
        }
    }
}
