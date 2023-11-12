using UnityMVVM.DI.Mapper;

namespace UnityMVVM.DI.Environment
{
    internal class ContainerEnvironment : IContainerEnvironment
    {
        public IViewToViewModelMutableMapper Mapper { get; }
        public IViewsContainerAdapter ViewsContainerAdapter { get; }
        public IViewsModelsContainerAdapter ViewsModelsContainerAdapter { get; }

        public ContainerEnvironment(
            IViewToViewModelMutableMapper mapper, 
            IViewsContainerAdapter viewsContainerAdapter, 
            IViewsModelsContainerAdapter viewsModelsContainerAdapter)
        {
            Mapper = mapper;
            ViewsContainerAdapter = viewsContainerAdapter;
            ViewsModelsContainerAdapter = viewsModelsContainerAdapter;
        }
    }
}