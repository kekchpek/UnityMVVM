using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMVVM.QuickStart
{
    public class MyViewBehaviour : ViewBehaviour<IMyViewModel>
    {

        // The component to display a text.
        [SerializeField] private TMP_Text _text;
        // The button component.
        [SerializeField] private Button _changeTextButton;
        
        // Initialize all subscriptions and bindings between view and view-model.
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            // Set text handler.
            ViewModel!.Text.Bind(OnTextChanged);
            // Subscribe for button click.
            _changeTextButton.onClick.AddListener(ViewModel!.OnTextChangeClick);
        }

        // Handler for view-model text value changing.
        private void OnTextChanged(string text)
        {
            _text.text = text;
        }

        // Release all subscriptions and bindings between view and view-model.
        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            // Remove text handler.
            ViewModel!.Text.Unbind(OnTextChanged);
            // Unsubscribe for button click.
            _changeTextButton.onClick.RemoveListener(ViewModel!.OnTextChangeClick);
        }
    }
}
