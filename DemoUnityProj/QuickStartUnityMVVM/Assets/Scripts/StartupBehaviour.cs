
using UnityEngine;
using UnityMVVM.ViewManager;
using Zenject;

namespace UnityMVVM.QuickStart
{
    public class StartupBehaviour : MonoBehaviour
    {
        private IViewManager _viewManager;
        
        [Inject]
        public void Construct(IViewManager viewManager) // IViewManager is bound automatically
        {
            _viewManager = viewManager;
        }
        
        private void Start()
        {
            _viewManager.Open(
                "Ui", // Layer to open a view on.
                "MyView" // The name of view to open.
            );
        }
    }
}
