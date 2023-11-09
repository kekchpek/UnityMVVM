using System;
using CCG.Core;
using CCG.MVVM.CoolPopup;
using CCG.MVVM.MainMenu;
using CCG.Tests.Editor.Core;
using CCG.Tests.Editor.Core.SynchronizationContext;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityMVVM.ViewModelCore;

namespace CCG.Tests.Editor
{
    public class CoolPopupTests
    {
        [Test]
        public void PopupButtonPressed_PopupOpened()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();
            
            // Assert
            testApp.AssertViewOpened(ViewLayerIds.Popup, ViewNames.CoolPopup);
        }
        
        [Test]
        public void PopupButtonPressed_PopupPropertiesFulfilled()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();
            var popup = testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup);
            
            // Assert
            Assert.IsTrue(popup.IsClosingAnimationActive, "Closing animation should be active on popup opens");
        }
        
        [Test]
        public void PopupCloseButton_PopupClosed()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();
            testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup).OnCloseBtn();
            
            // Assert
            testApp.AssertNoView(ViewLayerIds.Popup);
        }
        
        [Test]
        public void PopupOpenWithError_ErrorRaised()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            var testApp = TestApplication.Create();
            
            // Act
            // Open popup
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();
            // Open new popup with an error
            var popup = testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup);
            popup.OnOpenCoolPopupWithErrorBtn();

            // Assert
            Assert.Throws(Is.AssignableTo<Exception>(), TestSynchronizationContext.ExecutePendingTasks);
        }
        
        [Test]
        public void PopupOpenNew_OldOneDestroyed()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            var testApp = TestApplication.Create();
            bool isDestroyed = false;
            
            // Act
            // Open popup
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();

            // Open new popup and handle old destruction
            var popup = testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup);
            void Destroyed(IViewModel _)
            {
                isDestroyed = true;
            }
            popup.Destroyed += Destroyed;
            popup.OnOpenCoolPopupBtn();
            popup.Destroyed -= Destroyed;

            // Assert
            Assert.IsTrue(isDestroyed);
        }
        
        [Test]
        public void PopupOpenNew_NewPopupOpened()
        {
            // Arrange
            LogAssert.ignoreFailingMessages = true;
            var testApp = TestApplication.Create();
            
            // Act
            // Open popup
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();

            // Open new popup
            var popup = testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup);
            popup.OnOpenCoolPopupBtn();

            // Assert
            Assert.AreNotEqual(popup, testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup));
        }
        
        [Test]
        public void PopupClosingAnimationChanged_PropertyChanged()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OnCoolPopupBtn();
            var popup = testApp.GetViewModel<ICoolPopupViewModel>(ViewLayerIds.Popup);
            popup.SetClosingAnimationActive(false);
            
            // Assert
            Assert.IsFalse(popup.IsClosingAnimationActive, "Closing animation property is not changed!");
        }
    }
}