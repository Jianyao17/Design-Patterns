using System.Collections;
using DesignPattern.Singleton;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DesignPatterns.Tests.Singleton
{

    public class RegulatorSingletonTests
    {
        // Dummy test class inheriting from RegulatorSingleton
        public class TestRegulatorSingleton : RegulatorSingleton<TestRegulatorSingleton>
        {
            public bool AwakeCalled { get; private set; }

            protected override void Awake()
            {
                base.Awake();
                AwakeCalled = true; // Flag to verify if Awake was called
            }
        }

        [SetUp]
        public void SetUp()
        {
            // Cleanup before each test
            var existingInstances = Object.FindObjectsOfType<TestRegulatorSingleton>();
            foreach (var instance in existingInstances)
            {
                Object.DestroyImmediate(instance.gameObject);
            }
        }

        [UnityTest]
        public IEnumerator DontDestroyOnLoad_WorksAsExpected()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestRegulatorSingleton));
            var singleton = gameObject.AddComponent<TestRegulatorSingleton>();

            // Act
            var wasAddedToDontDestroyOnLoad = singleton.gameObject.scene.name == "DontDestroyOnLoad";

            // Assert
            yield return null;
            Assert.IsTrue(wasAddedToDontDestroyOnLoad, "GameObject should be moved to DontDestroyOnLoad scene.");
        }

        [UnityTest]
        public IEnumerator InitializeSingleton_IsCalled_OnAwake()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestRegulatorSingleton));

            // Act
            var singleton = gameObject.AddComponent<TestRegulatorSingleton>();

            // Assert
            yield return null;
            Assert.IsTrue(singleton.AwakeCalled, "Awake should call InitializeSingleton.");
        }

        [UnityTest]
        public IEnumerator NewSingleton_DestroysOldInstances_AndUpdatesInstanceReference()
        {
            // Arrange
            var oldSingleton = new GameObject(nameof(TestRegulatorSingleton)).AddComponent<TestRegulatorSingleton>();
            var oldInstanceReference = TestRegulatorSingleton.Instance;
            
            // Wait one frame to allow Unity to process Destroy
            yield return null;
            
            // Act
            var newSingleton = new GameObject(nameof(TestRegulatorSingleton)).AddComponent<TestRegulatorSingleton>();
            var newInstanceReference = TestRegulatorSingleton.Instance;

            // Assert
            Assert.IsTrue(oldSingleton == null, "Old singleton instance should be destroyed.");
            Assert.AreEqual(newSingleton, newInstanceReference, "Instance reference should update to the new singleton.");
        }

        [UnityTest]
        public IEnumerator Instance_WhenNull_FindsExistingInstance()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestRegulatorSingleton));
            var singleton = gameObject.AddComponent<TestRegulatorSingleton>();

            // Act
            var instance = TestRegulatorSingleton.Instance;

            // Assert
            yield return null;
            Assert.AreEqual(singleton, instance, "Instance should find the existing singleton.");
        }

        [UnityTest]
        public IEnumerator Instance_WhenNullAndNoExistingInstance_CreatesNewInstance_WithHideAndDontSave()
        {
            // Act
            var instance = TestRegulatorSingleton.Instance;

            // Assert
            yield return null;
            Assert.IsNotNull(instance, "Instance should create a new singleton if none exists.");
            Assert.AreEqual(HideFlags.HideAndDontSave, instance.gameObject.hideFlags, "New singleton GameObject should have HideAndDontSave flag.");
        }

        [UnityTest]
        public IEnumerator HasInstance_ReturnsTrue_WhenInstanceExists()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestRegulatorSingleton));
            gameObject.AddComponent<TestRegulatorSingleton>();

            // Act
            var hasInstance = TestRegulatorSingleton.HasInstance;

            // Assert
            yield return null;
            Assert.IsTrue(hasInstance, "HasInstance should return true when a singleton exists.");
        }

        [UnityTest]
        public IEnumerator HasInstance_ReturnsFalse_WhenInstanceDoesNotExist()
        {
            // Act
            var hasInstance = TestRegulatorSingleton.HasInstance;

            // Assert
            yield return null;
            Assert.IsFalse(hasInstance, "HasInstance should return false when no singleton exists.");
        }
    }

}