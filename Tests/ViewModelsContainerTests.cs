using NSubstitute;
using NUnit.Framework;
using System;
using UnityMVVM.ViewModelCore;
using UnityMVVM.ViewModelCore.ViewModelsContainer;
using UnityMVVM.ViewModelCore.ViewModelsFactory;

namespace Tests
{
    public class ViewModelsContainerTests
    {

        [Test]
        public void ViewModelCreated_OneModel_ResolvedSuccessfully()
        {
            // Arrange
            var container = TestsHelper.CreateContainerFor<ViewModelsContainer<IViewModel>>(out var viewModelsContainer);
            var viewModelsFactory = container.Resolve<IViewModelFactoryInternal<IViewModel>>();

            // Act
            viewModelsContainer.Initialize();
            var viewModelSub = Substitute.For<IViewModel>();
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(viewModelSub);
            var resolvedViewModel = viewModelsContainer.Resolve();

            // Assert
            Assert.That(resolvedViewModel, Is.EqualTo(viewModelSub));
        }

        [Test]
        public void ViewModelCreated_AndDestroyed_ResolvedNull()
        {
            // Arrange
            var container = TestsHelper.CreateContainerFor<ViewModelsContainer<IViewModel>>(out var viewModelsContainer);
            var viewModelsFactory = container.Resolve<IViewModelFactoryInternal<IViewModel>>();

            // Act
            viewModelsContainer.Initialize();
            var viewModelSub = Substitute.For<IViewModel>();
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(viewModelSub);
            viewModelSub.OnDestroy += Raise.Event<Action>();
            var resolvedViewModel = viewModelsContainer.Resolve();

            // Assert
            Assert.IsNull(resolvedViewModel);
        }

        [Test]
        public void ViewModelCreated_TwoModelsAndOneDestroyed_ResolvedSuccessfully()
        {
            // Arrange
            var container = TestsHelper.CreateContainerFor<ViewModelsContainer<IViewModel>>(out var viewModelsContainer);
            var viewModelsFactory = container.Resolve<IViewModelFactoryInternal<IViewModel>>();

            // Act
            viewModelsContainer.Initialize();
            var viewModelSub1 = Substitute.For<IViewModel>();
            var viewModelSub2 = Substitute.For<IViewModel>();
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(viewModelSub1);
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(viewModelSub2);
            viewModelSub1.OnDestroy += Raise.Event<Action>();
            var resolvedViewModel = viewModelsContainer.Resolve();

            // Assert
            Assert.That(resolvedViewModel, Is.EqualTo(viewModelSub2));
        }
        
        [Test]
        public void ViewModelCreated_TwoModels_Exception()
        {
            // Arrange
            var container = TestsHelper.CreateContainerFor<ViewModelsContainer<IViewModel>>(out var viewModelsContainer);
            var viewModelsFactory = container.Resolve<IViewModelFactoryInternal<IViewModel>>();

            // Act
            viewModelsContainer.Initialize();
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(Substitute.For<IViewModel>());
            viewModelsFactory.ViewModelCreated += Raise.Event<Action<IViewModel>>(Substitute.For<IViewModel>());

            // Assert
            Assert.Throws<Exception>(() => viewModelsContainer.Resolve());
        }
    }
}