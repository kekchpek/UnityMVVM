# UnityMVVM
The MVVM pattern core implemented for Unity3d. 

It is implemented over Zenject plugin. You have to add Zenject to your project to use it. (https://github.com/modesttree/Zenject)

## Concept
<details>
<summary>Concept</summary>

<p></p>
  
UnityMVVM is based on widely known MVVM pattern of application structure. It follows a layered architecture pattern, where all objects are supposed to be a part of a specific logical layer. UnityMVVM by default supports three layers - View(Interaction), ViewModel and Model.

<p align="center">
<img align="center" src="https://github.com/kekchpek/UnityMVVM/assets/18449140/14d5ae95-75bf-4038-9893-02a02c61d6e0"/>
</p>

- View(Interaction) layer contains Unity3d objects, that are responsible for content displaying. These objects also could handle interactions with user like buttons clicks and interactions with UnityAPI like `OnCollisionEnter`, `OnTriggerEnter` methods and Unity3d physics handling. This layer is not intended to store any data about its state or business logic. **View layer objects are inheritors of `ViewBehaviour` class.**
- ViewModel layer contains the data about View objects state. It is responsible for handling input from View layer and contain logic of handling Model layer entities state changing. **ViewModel layer objects are inheritors of `ViewModel` class.**
- Model layer contains business logic, interaction with persistent storage, network or domain model. Model layer objects could have any type.

The specific thing about UnityMVVM approach is that each View object has only one corresponding ViewModel object. So every entity(units, windows, popups and whatever you want) are a View-ViewModel pair. ViewModel could have dependency on any number of model layer entities.

<p align="center">
<img align="center" src="https://github.com/kekchpek/UnityMVVM/assets/18449140/ab00b2d5-fd5a-4bee-a237-cc7dd5ea0c81"/>
</p>

You can install View-ViewModel pair to the DI container with `InstallView` method. 
```csharp
Container.InstallView<MyViewBehaviour, IMyViewModel, MyViewModel>("MyView", _myViewPrefab); // Check Quick Start topic for details.
```

There is a `FastBind` method(and its overloads), that allows you to bind singletons and separate Model and ViewModel access interfaces. Like this:
```csharp
Container.FastBind<ICommonAccessInterface, OtherImplementation>(); // Common interface for model and view model layers.
Container.FastBind<IModelInterface, IViewModelInterface, Implementation>(); // Separated access.
```

Also, you can add Model layer entities with regular DI container API. But entities, that are added this way will not be accessible from the ViewModel layer. To make ViewModel layer able to use these entities use `ProvideAccessForViewModelLayer<T>()` method. Like this:
```csharp
Container.Bind<ISomeInterface>().To<Implementation>().AsSingle();
Container.ProvideAccessForViewModelLayer<ISomeInterface>();
```

Views and ViewModels also can have own layer-specific dependencies. To install them you can directly access View and ViewModel child containers with methods: `GetViewsContainer()` and `GetViewModelsContainer()`.

When you open or create a view, you can provide some data to created ViewModel with payloads. To receive the payload into ViewModel, just add it to its constructor. Like this:
```csharp
_viewManager.Open(
    "Ui", // Layer to open a view on.
    "MyView", // The name of view to open.
    new SomePayload(/* payload args */)
);

...

public class MyViewModel : ViewModel, IMyViewModel {

    public MyViewModel(ISomePayload payload) {
        // do something with payload
    }

}

```

So our scheme now looks like this:

<p align="center">
<img align="center" src="https://github.com/kekchpek/UnityMVVM/assets/18449140/1dacf75e-97d5-49b8-8a91-b160b5f0744c"/>
</p>

In addition, I would like to say that ViewModel- and View-only dependencies is not a good practice in terms of UnityMVVM approach, so be careful and do not overuse Views and ViewModels containers.

</details>

## Quick start
<details>
<summary>Quick start</summary>

<p></p>
  
This section provides a brief overview of how to create and open view. All example code could be found in demo project -https://github.com/kekchpek/UnityMVVM/tree/master/DemoUnityProj/QuickStartUnityMVVM

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
        
        private async void Start()
        {
            await _viewManager.Open(
                "Ui", // Layer to open a view on.
                "MyView" // The name of view to open.
                );
        }
    }
```

</details>
