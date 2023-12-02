# AsyncReactAwait (Ara)
**Ara** is the development suit for asynchronous and reactive C# development. It contains classes for convenient data changing and handling management.

## Bindable
`IBindable` and `IMutable` interfaces are supposed to be used for convenient changes handling of fields and properties
### Example
```csharp
public class HumanModel 
{
  private IMutable _age = new Mutable();
  public IBindable Age => _age; // keep age being mutable only from inside the class
  
  private IMutable _name = new Mutable("No-Name");
  public IBindable Name => _name; // keep name being mutable only from inside the class
  
  public void SetName(string newName) 
  {
    _name.Value = newName;
  }
  
  public void IncrementAge() 
  {
    _age.Value++;
  }
  
}

public class HumanView : IDisposable
{

  private readonly SomeLabel _ageLabel = new SomeLabel();
  private readonly SomeLabel _nameLabel = new SomeLabel();
  
  private readonly HumanModel _model;

  public HumanView(HumanModel model) 
  {
    _model = model;
    _model.Age.Bind(_ageLabel.SetText);
    _model.Name.Bind(_nameLabel.SetText);
  }
  
  public void Dispose()
  {
    _model.Age.Unbind(_ageLabel.SetText);
    _model.Name.Unbind(_nameLabel.SetText);
  }

}
```


Also `IBindable` have methods for awaiting some specific value.
### Example with `async`
```csharp
public class ScoreModel 
{
  private IMutable<int> _score = new Mutable();
  public IBindable Score => _score;
  
  public void AddScore(int score) 
  {
    _score += score;
  }
  
  public async void AddBonus(int bonusValue, int requiredScore) 
  {
    await Score.WillBe(s => 
      s >= requiredScore);
    AddScore(bonusValue);
  }
}
```

## Promises
Promises are some kind of a replacement for regular C# `Task`. They can also be used as a return type for `async` methods or with `await` like tasks. 
But promises have two advantages:
 - Promises have an interface. So you can easely substitute them to some other implementation.
 - Promises' completition is easier to control from managed code with `Success` and `Fail` methods.
### Example
```csharp

// The third party class for playing animations
public abstract class ThirdPartyAnimation 
{
  protected void Start() {...}
  protected virtual void Ended() {...}
}

// Managed animation implementation.
public class Animation : ThirdPartyAnimation 
{

  private IControllablePromise _playingPromise;

  public async IPromise Play() {
    _playingPromise = new ControllablePromise();
    Start();
    await _playingPromise;
  }
  
  protected override void Ended() {
    base.Ended();
    _playingPromise?.Success();
    _playingPromise = null;
  }
  
}
```
