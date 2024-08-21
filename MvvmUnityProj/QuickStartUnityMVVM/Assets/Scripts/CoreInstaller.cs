using UnityEngine;
using UnityMVVM.DI;
using Zenject;

namespace UnityMVVM.QuickStart
{
    public class CoreInstaller : MonoInstaller
    {

        // Some transform to place view objects into.
        [SerializeField] private Transform _uiRoot;

        // Prefab of the view with MyViewBehaviour component on it.
        [SerializeField] private GameObject _myViewPrefab;
        
        public override void InstallBindings()
        {
            // Firstly, we should initialize our container to make it suitable for UnityMVVM environment.
            // At this moment IViewManager is being added to the container.
            Container.UseAsMvvmContainer(new []
            {
                // We should specify our layers(containers for views) on initialization. Each layer has a name/id and transform (parent for view objects)
                ("Ui", _uiRoot),
            });

            // Each view is strongly dependent on its view model. So for view binding we should specify both view and view model implementation.
            // Also view is bound with view-model via interface, so we should specify view-model interface too.
            // And the last thing we should specify is view name/id(we will create views via these ids) and view prefab(prefab should have component of MyViewBehaviour type)
            Container.InstallView<MyViewBehaviour, IMyViewModel, MyViewModel>("MyView", _myViewPrefab); 
        }
    }
}
