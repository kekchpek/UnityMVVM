using CCG.Core;
using CCG.MVVM.CoolPopup;
using CCG.MVVM.MainMenu;
using CCG.Tests.Editor.Core;
using NUnit.Framework;

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
    }
}