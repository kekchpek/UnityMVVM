using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.ViewManager.ViewLayer
{
    internal class ViewLayerImpl : IViewLayer
    {

        [AllowNull, CanBeNull]
        private IViewModel _currentViewModel;

        public string Id { get; }

        public Transform Container { get; }

        public ViewLayerImpl(string id, Transform container)
        {
            Id = id;
            Container = container;
        }

        public void Clear()
        {
            _currentViewModel?.Destroy();
        }

        public void Set(IViewModel viewModel)
        {
            Clear();
            _currentViewModel = viewModel;
        }
    }
}
