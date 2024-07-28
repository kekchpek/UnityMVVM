using System;
using AsyncReactAwait.Bindable;
using CCG.Core;
using CCG.MVVM.MainScreen.ViewModel;
using CCG.MVVM.TimeCounter;
using CCG.Tests.Editor.Core;
using NSubstitute;
using NUnit.Framework;
using SurvivedWarrior.MVVM.Models.Time;

namespace CCG.Tests.Editor
{
    public class TimeCounterTests
    {
        [Test]
        public void TimeChanged_TimeCounted()
        {
            // Arrange
            var timeManager = Substitute.For<ITimeManager>();
            var timeProp = new Mutable<long>();
            timeManager.CurrentTimestamp.Returns(timeProp);
            var testApp = TestApplication.Create(rebindMap: new []
            {
                (typeof(ITimeManager), (object)timeManager)
            });
            
            // Act
            testApp.Start();
            testApp.ClickPlayButton();
            var counter = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<ITimeCounterViewModel>();
            timeProp.Value += TimeSpan.TicksPerSecond;

            // Assert
            Assert.AreEqual(1f, counter.TimeInSeconds.Value);
        }
        
        [Test]
        public void Initialization_TimeIsZero()
        {
            // Arrange
            var timeManager = Substitute.For<ITimeManager>();
            var testApp = TestApplication.Create(rebindMap: new []
            {
                (typeof(ITimeManager), (object)timeManager)
            });
            
            // Act
            testApp.Start();
            testApp.ClickPlayButton();
            var counter = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI)
                .GetSubview<ITimeCounterViewModel>();

            // Assert
            Assert.AreEqual(0f, counter.TimeInSeconds.Value);
        }
        
        [Test]
        public void TimeChanged_ButPopupOpened_TimeNotCounted()
        {
            // Arrange
            var timeManager = Substitute.For<ITimeManager>();
            var timeProp = new Mutable<long>();
            timeManager.CurrentTimestamp.Returns(timeProp);
            var testApp = TestApplication.Create(rebindMap: new []
            {
                (typeof(ITimeManager), (object)timeManager)
            });
            
            // Act
            testApp.Start();
            testApp.ClickPlayButton();
            var mainScreen = testApp
                .GetViewModel<IMainScreenViewModel>(ViewLayerIds.MainUI);
            var counter = mainScreen
                .GetSubview<ITimeCounterViewModel>();
            mainScreen.OnPopupButtonClicked();
            timeProp.Value += 1000;

            // Assert
            Assert.AreEqual(0f, counter.TimeInSeconds.Value);
        }
    }
}