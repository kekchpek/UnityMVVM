using System;
using UnityEngine;
using UnityMVVM.DI.Mapper;

namespace UnityMVVM.DI.Environment
{
    /// <summary>
    /// The context of MVVM container, that contains auxiliary objects.
    /// </summary>
    internal interface IContainerEnvironment
    {
        IViewToViewModelMutableMapper Mapper { get; }
        IViewsContainerAdapter ViewsContainerAdapter { get; }
        IViewsModelsContainerAdapter ViewsModelsContainerAdapter { get; }
    }
}