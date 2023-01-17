using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityMVVM.ViewModelCore.Bindable
{

    /// <summary>
    /// Class for representing changable bindable value.
    /// </summary>
    /// <typeparam name="T">Bindable value type.</typeparam>
    public interface IMutable<T> : IBindable<T>
    {

        /// <inheritdoc cref="IBindable{T}.Value"/>
        new T Value { get; set; }
    }
}
