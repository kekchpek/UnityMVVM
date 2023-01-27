using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityAuxiliaryTools.Promises;
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

        public IPromise Clear()
        {
            if (_currentViewModel == null)
            {
                var promise = new ControllablePromise();
                promise.Success();
                return promise;
            }
            return _currentViewModel.Close();
        }

        public void ClearInstantly()
        {
           _currentViewModel.Destroy();
        }

        public void Set(IViewModel viewModel)
        {
            Clear();
            _currentViewModel = viewModel;
            _currentViewModel.Destroyed += OnViewModelDestoryed;
        }

        private void OnViewModelDestoryed()
        {
            _currentViewModel.Destroyed -= OnViewModelDestoryed;
            _currentViewModel = null;
        }
    }
}
