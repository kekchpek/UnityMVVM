using System;
using System.Collections.Generic;
using System.Linq;
using CCG.Core;
using CCG.MVVM.MainMenu;
using CCG.MVVM.SubviewsColorChanger;
using CCG.MVVM.SubviewsColorChanger.Color;
using CCG.Tests.Editor.Core;
using NUnit.Framework;

namespace CCG.Tests.Editor
{
    public class ColorChangerTests
    {

        [Test]
        public void Opened()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OpenColorChanger();
            
            // Assert
            testApp.AssertViewOpened(ViewLayerIds.Popup, ViewNames.ColorChanger);
        }

        [Test]
        public void ClickedOnce_SubviewColorChanged()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OpenColorChanger();
            var subviews = testApp.GetViewModel<IColorChangerViewModel>(ViewLayerIds.Popup).GetSubviews<IColorViewModel>().ToArray();
            var colorsBefore = subviews.Select(x => x.Color.Value).ToArray();
            testApp.GetViewModel<IColorChangerViewModel>(ViewLayerIds.Popup).ChangeColor();
            var colorsAfter = subviews.Select(x => x.Color.Value).ToArray();
            
            // Assert
            AssertOneElementChanged(colorsBefore, colorsAfter);
        }

        [Test]
        public void Closed()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start();
            testApp.GetViewModel<IMainMenuViewModelUi>(ViewLayerIds.MainUI).OpenColorChanger();
            testApp.GetViewModel<IColorChangerViewModel>(ViewLayerIds.Popup).Close();

            // Assert
            testApp.AssertNoView(ViewLayerIds.Popup);
        }
        
        static void AssertOneElementChanged<T>(IReadOnlyCollection<T> arr1, IReadOnlyCollection<T> arr2)
        {
            if (arr1.Count != arr2.Count)
            {
                throw new ArgumentException("Arrays must have the same length");
            }

            var groupedDiff = arr1.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            foreach (var num in arr2)
            {
                if (groupedDiff.ContainsKey(num))
                {
                    groupedDiff[num]--;
                    if (groupedDiff[num] == 0)
                    {
                        groupedDiff.Remove(num);
                    }
                }
                else
                {
                    groupedDiff[num] = -1;
                }
            }

            if (groupedDiff.Values.Sum(Math.Abs) != 2)
            {
                throw new InvalidOperationException("Exactly one element must be different");
            }
        }
        
    }
}