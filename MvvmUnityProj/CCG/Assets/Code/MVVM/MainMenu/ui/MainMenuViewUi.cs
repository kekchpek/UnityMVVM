using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace CCG.MVVM.MainMenu
{
    public class MainMenuViewUi : ViewBehaviour<IMainMenuViewModelUi>
    {

        [SerializeField] private Button _playButton;
        
        [SerializeField] private Button _backButton;

        [SerializeField] private Button _cubeButton;
        [SerializeField] private Button _sphereButton;
        [SerializeField] private Button _cylinderButton;
        [SerializeField] private Button _capsuleButton;
        [SerializeField] private Button _coolPopupButton;
        [SerializeField] private Button _colorChangerButton;
        
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel!.IsInteractable.Bind(OnInteractableChanged);
            ViewModel!.PlayButtonShown.Bind(OnPlayButtonShownChange);
            ViewModel!.StatesButtonsShown.Bind(OnStatesButtonsShownChanged);
            ViewModel!.BackButtonShown.Bind(OnBackButtonShownChanged);
            _cubeButton.onClick.AddListener(() => ViewModel!.OnSwitchStateButtonPressed(MainMenuState.Cube));
            _sphereButton.onClick.AddListener(() => ViewModel!.OnSwitchStateButtonPressed(MainMenuState.Sphere));
            _cylinderButton.onClick.AddListener(() => ViewModel!.OnSwitchStateButtonPressed(MainMenuState.Cylinder));
            _capsuleButton.onClick.AddListener(() => ViewModel!.OnSwitchStateButtonPressed(MainMenuState.Capsule));
            _backButton.onClick.AddListener(() => ViewModel!.OnSwitchStateButtonPressed(MainMenuState.None));
            _coolPopupButton.onClick.AddListener(() => ViewModel!.OnCoolPopupBtn());
            _colorChangerButton.onClick.AddListener(() => ViewModel!.OpenColorChanger());
        }

        private void OnPlayButtonShownChange(bool shown)
        {
            _playButton.gameObject.SetActive(shown);
        }
        
        private void OnBackButtonShownChanged(bool shown)
        {
            _backButton.gameObject.SetActive(shown);
        }
        
        private void OnStatesButtonsShownChanged(bool shown)
        {
            _cubeButton.gameObject.SetActive(shown);
            _sphereButton.gameObject.SetActive(shown);
            _cylinderButton.gameObject.SetActive(shown);
            _capsuleButton.gameObject.SetActive(shown);
        }

        private void OnInteractableChanged(bool isInteractable)
        {
            _playButton.interactable = isInteractable;
            _cubeButton.interactable = isInteractable;
            _sphereButton.interactable = isInteractable;
            _cylinderButton.interactable = isInteractable;
            _capsuleButton.interactable = isInteractable;
            _backButton.interactable = isInteractable;
            _coolPopupButton.interactable = isInteractable;
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            ViewModel!.IsInteractable.Unbind(OnInteractableChanged);
            ViewModel.PlayButtonShown.Unbind(OnPlayButtonShownChange);
            ViewModel.StatesButtonsShown.Unbind(OnStatesButtonsShownChanged);
            ViewModel.BackButtonShown.Unbind(OnBackButtonShownChanged);
            _playButton.onClick.RemoveAllListeners();
            _cubeButton.onClick.RemoveAllListeners();
            _sphereButton.onClick.RemoveAllListeners();
            _cylinderButton.onClick.RemoveAllListeners();
            _capsuleButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _coolPopupButton.onClick.RemoveAllListeners();
            _colorChangerButton.onClick.RemoveAllListeners();
        }
    }
}