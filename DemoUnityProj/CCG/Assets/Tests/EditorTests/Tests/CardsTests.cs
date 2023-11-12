using System.Linq;
using CCG.Core;
using CCG.Models.Hand.Model;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Card.ViewModel;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.Tests.Editor.Core;
using NSubstitute;
using NUnit.Framework;
using UnityMVVM.ViewModelCore;

namespace CCG.Tests.Editor
{
    public class CardsTests
    {

        [Test]
        public void Fail()
        {
            Assert.Fail();
        }
        
        [Test]
        public void GameStart_NoSelectedCards()
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
            Assert.IsTrue(cards.All(x => !x.IsSelected.Value), "Some card is selected right after game start.");
        }
        
        [Test]
        public void ClickDownCard_CardSelected()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click down to any card.
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            cards[0].OnMouseClickDown();

            // Assert
            Assert.IsTrue(cards[0].IsSelected.Value, "Card is not selected after clicking on it.");
        }
        
        [Test]
        public void ClickDownCard_ClickUp_CardDeselected()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click down to any card.
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            cards[0].OnMouseClickDown();
            cards[0].OnMouseClickUp();

            // Assert
            Assert.IsFalse(cards[0].IsSelected.Value, "Card is still selected after click release on it.");
        }
        
        [Test]
        public void DragCard_OverBoard_CardDestroyed()
        {
            // Arrange
            var testApp = TestApplication.Create();
            bool isDestroyed = false;
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click down to any card.
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();
            void CardDestroyed(IViewModel vm)
            {
                isDestroyed = true;
            }

            cards[0].Destroyed += CardDestroyed;
                
            cards[0].OnMouseClickDown();
            cards[0].OnCardEnterBoard();
            cards[0].OnMouseClickUp();
            
            cards[0].Destroyed -= CardDestroyed;

            // Assert
            Assert.IsTrue(isDestroyed, "Card is not destroyed after playing on the board.");
        }
        
        [Test]
        public void DragCard_OverBoard_CardPlayed()
        {
            // Arrange
            // Create single card.
            var card = Substitute.For<ICardModel>();
            
            // Create hand model for this card.
            var handModel = Substitute.For<IHandModel>();
            handModel.CardsCount.Returns(1);
            handModel.MaxCardsCount.Returns(10);
            handModel.GetCards().Returns(new[] { card });
            
            // Create test app.
            var testApp = TestApplication.Create(rebindMap: new []
            {
                (typeof(IHandModel), (object)handModel)
            });
            
            // Act
            testApp.Start();
            
            // Click play button.
            testApp.ClickPlayButton();
            
            // Click down to any card.
            var cards = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubviews<ICardViewModel>();

            cards[0].OnMouseClickDown();
            cards[0].OnCardEnterBoard();
            cards[0].OnMouseClickUp();
            
            // Assert
            card.Received().Play();
        }
    }
}