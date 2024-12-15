using CCG.Core;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.HandController;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.Tests.Editor.Core;
using NUnit.Framework;
using UnityMVVM.ViewModelCore;

namespace CCG.Tests.Editor
{
    public class MainScreenTests
    {
        [Test]
        public void MainScreenOpened_AddCardClicked_SubviewCreated()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click add card
            var mainScreen = testApp.GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI);
            IViewModel parent = null;
            IViewModel created = null;
            void OnSubviewCreated(IViewModel p, IViewModel child)
            {
                parent = p;
                created = child;
            }

            mainScreen.SubviewCreated += OnSubviewCreated;
            mainScreen.GetSubview<IHandControllerViewModel>().AddRandomCardToHand();
            mainScreen.SubviewCreated -= OnSubviewCreated;

            // Assert
            Assert.AreEqual(parent, mainScreen);
            Assert.IsTrue(created is ICardViewModel);
        }
        
        [Test]
        public void MainScreenOpened_AddCardNotClicked_SubviewNotCreated()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click add card
            var mainScreen = testApp.GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI);
            var evtCalled = false;
            void OnSubviewCreated(IViewModel p, IViewModel child)
            {
                evtCalled = true;
            }
            mainScreen.SubviewCreated += OnSubviewCreated;

            // Assert
            Assert.IsFalse(evtCalled);
            
            mainScreen.SubviewCreated -= OnSubviewCreated;
        }
    }
}