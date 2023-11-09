using AsyncReactAwait.Promises;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CCG.Tests.Editor
{
    public class TestSubstitution
    {
        public interface ITestInterface
        {
            IPromise<object> GetReference();
            IPromise<int> GetValue();
            IPromise<Texture2D> GetUnityObject();
            IPromise GetNonResult();
        }

        [Test]
        public void ReferenceValuePromise_SuccessfulNull()
        {
            // Arrange
            var sub = Substitute.For<ITestInterface>();
            
            // Act
            var promise = sub.GetReference();
            
            // Assert
            if (promise.TryGetResult(out var result))
            {
                Assert.IsNull(result, "Promise result is not null.");
            }
            else
            {
                Assert.Fail("Promise is not successful.");
            }
        }

        [Test]
        public void ValuePromise_SuccessfulDefault()
        {
            // Arrange
            var sub = Substitute.For<ITestInterface>();
            
            // Act
            var promise = sub.GetValue();
            
            // Assert
            if (promise.TryGetResult(out var result))
            {
                Assert.AreEqual(0, result, "Promise result is not default value.");
            }
            else
            {
                Assert.Fail("Promise is not successful.");
            }
        }

        [Test]
        public void UnityObjectPromise_SuccessfulPredefined()
        {
            // Arrange
            var sub = Substitute.For<ITestInterface>();
            
            // Act
            var promise = sub.GetUnityObject();
            
            // Assert
            if (promise.TryGetResult(out var result))
            {
                Assert.AreEqual(Texture2D.blackTexture, result, "Promise result is not predefined value for unity object.");
            }
            else
            {
                Assert.Fail("Promise is not successful.");
            }
        }

        [Test]
        public void NonResultPromise_Successful()
        {
            // Arrange
            var sub = Substitute.For<ITestInterface>();
            bool isSuccessful = false;
            
            // Act
            var promise = sub.GetNonResult();
            promise.OnSuccess(() => isSuccessful = true);

            // Assert
            Assert.IsTrue(isSuccessful, "Promise is not successful.");
        }
    }
}