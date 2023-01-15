using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityMVVM.ViewModelCore
{
    public interface IMutable<T> : IBindable<T>
    {
        new T Value { get; set; }
    }
}
