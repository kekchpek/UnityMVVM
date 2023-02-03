using AsyncReactAwait.Bindable;

namespace CCG.Core.Camera
{
    public interface ICameraModel
    {
        IBindable<UnityEngine.Camera> CurrenCamera { get; }
    }
}