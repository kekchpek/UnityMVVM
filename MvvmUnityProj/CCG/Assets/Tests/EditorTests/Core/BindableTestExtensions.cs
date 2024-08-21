using AsyncReactAwait.Bindable;
using NSubstitute;
using NSubstitute.Core;

namespace CCG.Tests.Editor.Core
{
    public static class BindableTestExtensions
    {
        public static ConfiguredCall Returns<T>(this IBindable<T> bindable, T val)
        {
            return bindable.Value.Returns(val);
        }
    }
}