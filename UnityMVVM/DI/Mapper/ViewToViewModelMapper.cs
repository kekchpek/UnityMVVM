using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.ViewModelCore;

namespace UnityMVVM.DI.Mapper
{
    internal class ViewToViewModelMapper : IViewToViewModelMutableMapper
    {

        private readonly IDictionary<Type, Type> _viewToViewModelMap = new Dictionary<Type, Type>();

        public void Map<TView, TViewModel>()
            where TViewModel : class, IViewModel
            where TView : IViewBehaviour
        {
            MapInternal(typeof(TView), typeof(TViewModel), false);
        }

        public void Map(Type viewType, Type viewModelType)
        {
            MapInternal(viewType, viewModelType, true);
        }

        public Type GetViewModelForView(Type viewType)
        {
            if (_viewToViewModelMap.ContainsKey(viewType))
            {
                return _viewToViewModelMap[viewType];
            }
            throw new Exception($"Can not find corresponing view model type for {viewType.Name}");
        }

        private void MapInternal(Type viewType, Type viewModelType, bool validate)
        {
            if (validate)
            {
                if (!viewType.GetParentTypes().ContainsItem(typeof(MonoBehaviour)))
                {
                    throw new Exception($"View should inherit {nameof(MonoBehaviour)}");
                }
                if (!viewType.Interfaces().ContainsItem(typeof(IViewModel)))
                {
                    throw new Exception($"View model should implement {nameof(IViewModel)}");
                }
            }
            _viewToViewModelMap.Add(viewType, viewModelType);
        }
    }
}
