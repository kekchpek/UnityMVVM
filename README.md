# UnityMVVM
The MVVM pattern core implemented for Unity3d. 

It is implemented over Zenject plugin. You have to add Zenject to your project to use it. (https://github.com/modesttree/Zenject)

## Quick start

This is an example of simple installer with UnityMVVM plugin usage.
```csharp
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private Transform _uiRoot;

        [SerializeField] private GameObject _myViewPrefab;
        
        public override void InstallBindings()
        {
            Container.UseAsMvvmContainer(new []
            {
                ("Ui", _uiRoot),
            });
            Container.InstallView<MyViewBehaviour, IMyViewModel, MyViewModel>("MyView", _myViewPrefab);
        }
    }
```

After installing such installer you can add such `StartupBehaviour` behaviour to your scene to open the initial view.
```csharp
    public class StartupBehaviour : MonoBehaviour
    {
        private IViewManager _viewManager;
        
        [Inject]
        public void Construct(IViewManager viewManager) // IViewManager is being bound automatically
        {
            _viewManager = viewManager;
        }
        
        private void Start()
        {
            _viewManager.Open(
                "Ui", // Layer to open a view on
                "MyView" // The view name
                );
        }
    }
```
