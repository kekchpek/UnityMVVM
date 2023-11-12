using Zenject;

namespace UnityMVVM.DI
{
    internal class ViewsContainerAdapter : IViewsContainerAdapter
    {
        public DiContainer Container { get; }

        public ViewsContainerAdapter(DiContainer container)
        {
            Container = container;
        }

    }
}
