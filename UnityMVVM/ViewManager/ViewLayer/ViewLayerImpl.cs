using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{
    internal class ViewLayerImpl : IViewLayer
    {
        public string Id => throw new NotImplementedException();

        public Transform Container => throw new NotImplementedException();

        public ViewLayerImpl(string id, Transform container)
        {

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Set(IViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
