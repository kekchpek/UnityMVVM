using System.Linq;
using System.Text;
using CCG.Core;
using CCG.Models.Hand.Model;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.HandController;
using CCG.MVVM.MainMenu;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.PlayButton;
using CCG.Tests.Editor.Core;
using NUnit.Framework;

namespace CCG.Tests.Editor
{
    public class GameTests
    {
        [Test]
        public void GameStarted_MainScreenOpened()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Assert
            testApp.AssertViewOpened<IMainScreenViewModel>(ViewLayerIds.MainUI);
        }
        
        [Test]
        public void GameStarted_CardsCreated()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();

            // Assert
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            Assert.IsTrue(cards.Length > 0, "Cards were not created after game started.");
        }
        
        [Test]
        public void AddCard_CardAdded()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Store current cards to assert.
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            
            // Add one card.
            var handController = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<IHandControllerViewModel>();
            handController.AddRandomCardToHand();

            // Assert
            var newCards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            Assert.AreEqual(cards.Length + 1, newCards.Length, "Card is not added after click add card button.");
        }
        
        [Test]
        public void AddCards_OverMax_CardsCountIsMax()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Add Max+1 cards
            var handController = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<IHandControllerViewModel>();
            var cardsModel = testApp.GetViewModelDependency<IHandModel>();
            for (int i = 0; i < cardsModel.MaxCardsCount + 1; i++)
            {
                handController.AddRandomCardToHand();
            }

            // Assert
            var newCards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            Assert.AreEqual(cardsModel.MaxCardsCount, newCards.Length, "Card is not added after click add card button.");
        }
        
        [Test]
        public void SwitchCardsPattern_CardPositionIsArch()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Switch cards pattern.
            var handController = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<IHandControllerViewModel>();
            handController.SwitchCardsPattern();

            // Assert
            var handModel = testApp.GetViewModelDependency<IHandModel>();
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            for (int i = 0; i < handModel.CardsCount.Value; i++)
            {
                var data = CardMath.GetRegularTransform(
                    i,
                    handModel.CardsCount.Value);
                Assert.IsTrue(cards.Any(x => 
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        x.PositionInHand.Value == data.position && x.RotationInHand.Value == data.angle), 
                    "Card with expected arch position is not found.\n" +
                    $"Expected position and rotation : {data.position}; {data.angle}\n" +
                    "Cards positions and rotations:\n" +
                    cards.Aggregate(new StringBuilder(), (sb, x) => 
                        sb.Append($"{x.PositionInHand.Value}; {x.RotationInHand.Value}\n")));
            }
        }
        
        [Test]
        public void SwitchCardsPattern_AndSwitchBack_CardPositionIsArch()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Switch cards pattern
            var handController = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<IHandControllerViewModel>();
            handController.SwitchCardsPattern();
            
            //Switch cards pattern back
            handController.SwitchCardsPattern();

            // Assert
            var handModel = testApp.GetViewModelDependency<IHandModel>();
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            for (int i = 0; i < handModel.CardsCount.Value; i++)
            {
                var data = CardMath.GetArchTransform(
                    i,
                    handModel.CardsCount.Value,
                    handModel.MaxCardsCount);
                Assert.IsTrue(cards.Any(x => 
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        x.PositionInHand.Value == data.position && x.RotationInHand.Value == data.angle), 
                    "Card with expected arch position is not found.\n" +
                    $"Expected position and rotation : {data.position}; {data.angle}\n" +
                    "Cards positions and rotations:\n" +
                    cards.Aggregate(new StringBuilder(), (sb, x) => 
                        sb.Append($"{x.PositionInHand.Value}; {x.RotationInHand.Value}\n")));
            }
        }
        
        [Test]
        public void StartGameAndExit_MainMenuOpened()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click exit to main menu button.
            var mainScreen = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI);
            mainScreen.OnMainMenuButtonClicked();
            
            
            // Assert
            testApp.AssertViewOpened<IMainMenuViewModelUi>(ViewLayerIds.MainUI);
            testApp.AssertViewOpened<IMainMenuViewModel3d>(ViewLayerIds.Main3d);
        }

    }
}