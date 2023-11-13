# UnityMVVM
The MVVM pattern core implemented for Unity3d. 

It is implemented over Zenject plugin. You have to add Zenject to your project to use it. (https://github.com/modesttree/Zenject)

## Concept
<details>
<summary>Concept</summary>

UnityMVVM is based on widely known MVVM pattern of application structure. It is layeredd architecture pattern, where all objects are supposed to be a part of a specific logical layer. UnityMVVM by default supports three layers - View(Interaction), ViewModel and Model.

<p align="center">
<img align="center" src="https://github.com/kekchpek/UnityMVVM/assets/18449140/14d5ae95-75bf-4038-9893-02a02c61d6e0"/>
</p>

- View(Interaction) layer contains Unity3d objects, that are responsible for content displaying. These objects also could handle interactions with user like buttons clicks and interactions with UnityAPI like `OnCollisionEnter`, `OnTriggerEnter` methods and Unity3d physics handling. This layer is not supposed to store any data about its state or buisnes logic. **View layer objects are childs of `ViewBehaviour` class.**
- ViewModel layer contains the data about View objects state. It responsible for handling input from View layer and contain logic of hanling Model layer entities state changing. **ViewModel layer objects are childs of `ViewModel` class.**
- Model layer contains buiseness logic, interaction with persistent storage, network or domain model. Model layer objects could have any type.

The specific thing about UnityMVVM approach is that every View object has only one ViewModel object. So every entity(units, windows, popups and whatever you want) are a View-ViewModel pair. ViewModel could have dependency on any number of model layer entities.

<p align="center">
<img align="center" src="https://github.com/kekchpek/UnityMVVM/assets/18449140/ab00b2d5-fd5a-4bee-a237-cc7dd5ea0c81"/>
</p>


</details>

## Quick start
<details>
<summary>Quick start</summary>
  
This topic slightly shows how to create and open view. All example code could be found in demo project -https://github.com/kekchpek/UnityMVVM/tree/master/DemoUnityProj/QuickStartUnityMVVM

Firstly, we should create a view and its view-model to open anything. Lets create simple View-ViewModel pair.

The interface for view model:
```csharp
    public interface IMyViewModel : IViewModel
    {
        // The bindable string value.
        IBindable<string> Text { get; }
        
        // The handler for ui button click event.
        void OnTextChangeClick();
    }
```

The view model implementation:
```csharp
    public class MyViewModel : ViewModel, IMyViewModel
    {
        
        // Private accessor to mutate a string value.
        private readonly IMutable<string> _text = new Mutable<string>();
        
        // Public bind/read only access to string value.
        public IBindable<string> Text => _text;
        
        public void OnTextChangeClick()
        {
            // Set new random text.
            _text.Set(Guid.NewGuid().ToString());
        }
    }
```

The view behaviour:
```csharp

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
```

After we create a View-ViewModel pair, we should open our view in the game. To do this, we should install the view into Zenject container before.
This is an example of simple installer with UnityMVVM plugin usage.
```csharp
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
```

After installing such installer in some context you can add `StartupBehaviour` behaviour to your scene to open the installed view.
```csharp
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
```

</details>
