using CCG.Core;
using CCG.MVVM.MainMenu;
using CCG.Tests.Editor.Core;
using NUnit.Framework;

namespace CCG.Tests.Editor
{
    public class MainMenuTests
    {

        [Test]
        public void InitialState_PropertiesFulfilled()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            var menu = testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            
            // Assert
            Assert.IsTrue(menu.IsInteractable.Value, "Menu is not interactable after initialization.");
            Assert.IsFalse(menu.BackButtonShown.Value, "Back button is shown without changing state.");
            Assert.IsTrue(menu.PlayButtonShown.Value, "Play button is not shown after initialization.");
            Assert.IsTrue(menu.StatesButtonsShown.Value, "States button is not shown after initialization.");
        }

        [Test]
        public void SwitchState_SwitchCompleted_PropertiesFulfilled()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            var menu = testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            var menu3d = testApp.GetViewModel<IMainMenuViewModel3d>(ViewLayerIds.Main3d);
            
            // Start changing state
            menu.OnSwitchStateButtonPressed(MainMenuState.Capsule);
            
            // Complete state changing
            menu3d.OnStateChangeCompleted();
            
            // Assert
            Assert.IsTrue(menu.BackButtonShown.Value, "Back button is not shown after changing state.");
            Assert.IsFalse(menu.StatesButtonsShown.Value, "States button is shown after changing state.");
            Assert.IsFalse(menu.PlayButtonShown.Value, "Play button is not hid after switching state");
            Assert.IsTrue(menu.IsInteractable.Value, "Menu is not interactable after switching state");
        }

        [Test]
        public void SwitchState_SwitchNotCompleted_NotInteractable()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            var menu = testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            
            // Start changing state
            menu.OnSwitchStateButtonPressed(MainMenuState.Capsule);
            
            // Assert
            Assert.IsFalse(menu.IsInteractable.Value, "Menu is interactable while states changing.");
        }

        
        [Test]
        public void SwitchState_Back_PlayButtonShown()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            var menu = testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            var menu3d = testApp.GetViewModel<IMainMenuViewModel3d>(ViewLayerIds.Main3d);
            
            // Start state changing.
            menu.OnSwitchStateButtonPressed(MainMenuState.Capsule);
            
            // Complete state changing.
            menu3d.OnStateChangeCompleted();
            
            // Start state changing.
            menu.OnSwitchStateButtonPressed(MainMenuState.None);
            
            // Complete state changing.
            menu3d.OnStateChangeCompleted();
            
            // Assert
            Assert.IsTrue(menu.PlayButtonShown.Value, "Play button is still hid after switching state back");
        }
        
        [Test]
        public void SwitchState_3dModelStateChanged()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            var menu = testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            var menu3d = testApp.GetViewModel<IMainMenuViewModel3d>(ViewLayerIds.Main3d);
            
            menu.OnSwitchStateButtonPressed(MainMenuState.Capsule);
            
            // Assert
            Assert.AreEqual(menu3d.State.Value, MainMenuState.Capsule);
        }
    }
}