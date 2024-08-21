using CCG.Core;
using CCG.Tests.Editor.Core;
using NUnit.Framework;

namespace CCG.Tests.Editor
{
    public class StartupTests
    {
        [Test]
        public void Startup_MenuOpened()
        {
            // Arrange
            var testApp = TestApplication.Create();
            
            // Act
            testApp.Start().ThrowIfFailed();

            // Assert
            testApp.AssertViewOpened(ViewLayerIds.Main3d, ViewNames.MainMenu3d);
        }
        
    }
}