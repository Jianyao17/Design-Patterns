using System.Collections;
using NUnit.Framework;
using UnityEngine;
using DesignPattern.Singleton;
using UnityEngine.TestTools;

namespace DesignPatterns.Tests.Singleton
{
    public class PersistentSingletonTests
    {
        // Test class inheriting from PersistentSingleton for testing purposes
        public class TestPersistentSingleton : PersistentSingleton<TestPersistentSingleton>
        {
            public bool AwakeCalled { get; private set; }

            protected override void Awake()
            {
                base.Awake();
                AwakeCalled = true; // Flag to check if Awake was called
            }
        }

        [SetUp]
        public void SetUp()
        {
            // Clean up any existing singleton instances before each test
            var existing = Object.FindObjectOfType<TestPersistentSingleton>();
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }
        }

        [UnityTest]
        public IEnumerator DontDestroyOnLoad_WorksAsExpected()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestPersistentSingleton));
            var singleton = gameObject.AddComponent<TestPersistentSingleton>();

            // Act
            var wasAddedToDontDestroyOnLoad = singleton.gameObject.scene.name == "DontDestroyOnLoad";

            // Assert
            yield return null;
            Assert.IsTrue(wasAddedToDontDestroyOnLoad, "GameObject should be moved to DontDestroyOnLoad scene.");
        }

        [UnityTest]
        public IEnumerator Instance_Destroyed_IfNotTheSameInstance()
        {
            // Arrange
            var firstInstance = new GameObject(nameof(TestPersistentSingleton)).AddComponent<TestPersistentSingleton>();
            var secondInstance =
                new GameObject(nameof(TestPersistentSingleton)).AddComponent<TestPersistentSingleton>();

            // Wait one frame to allow Unity to process Destroy
            yield return null;
            
            // Act
            var isFirstStillAlive = firstInstance != null;
            var isSecondDestroyed = secondInstance == null;

            // Assert
            Assert.IsTrue(isFirstStillAlive, "First instance should remain alive.");
            Assert.IsTrue(isSecondDestroyed, "Second instance should be destroyed.");
        }

        [UnityTest]
        public IEnumerator GameObject_MovedToRoot_IfAutoUnparrentOnAwake()
        {
            // Arrange
            var parent = new GameObject("Parent");
            var child = new GameObject(nameof(TestPersistentSingleton));
            child.transform.SetParent(parent.transform);

            // Act
            var singleton = child.AddComponent<TestPersistentSingleton>();
            var isInRoot = singleton.transform.parent == null;

            // Assert
            yield return null;
            Assert.IsTrue(isInRoot, "GameObject should be moved to root when AutoUnparrentOnAwake is true.");
        }

        [UnityTest]
        public IEnumerator Awake_Calls_InitializeSingleton()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestPersistentSingleton));

            // Act
            var singleton = gameObject.AddComponent<TestPersistentSingleton>();

            // Assert
            yield return null;
            Assert.IsTrue(singleton.AwakeCalled, "Awake should call InitializeSingleton.");
        }

        [UnityTest]
        public IEnumerator Instance_WhenNull_FindsExistingInstance()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestPersistentSingleton));
            var singleton = gameObject.AddComponent<TestPersistentSingleton>();

            // Act
            var instance = TestPersistentSingleton.Instance;

            // Assert
            yield return null;
            Assert.AreEqual(singleton, instance, "Instance should return the existing instance.");
        }

        [UnityTest]
        public IEnumerator Instance_WhenNullAndNoExistingInstance_CreatesNewInstance()
        {
            // Act
            var instance = TestPersistentSingleton.Instance;

            // Assert
            yield return null;
            Assert.IsNotNull(instance, "Instance should create a new instance if none exists.");
            Assert.IsTrue(instance.name.Contains(nameof(TestPersistentSingleton)),
                "The new instance should have the correct name.");
        }

        [UnityTest]
        public IEnumerator HasInstance_ReturnsTrue_WhenInstanceExists()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestPersistentSingleton));
            gameObject.AddComponent<TestPersistentSingleton>();

            // Act
            var hasInstance = TestPersistentSingleton.HasInstance;

            // Assert
            yield return null;
            Assert.IsTrue(hasInstance, "HasInstance should return true when an instance exists.");
        }

        [UnityTest]
        public IEnumerator HasInstance_ReturnsFalse_WhenInstanceDoesNotExist()
        {
            // Act
            var hasInstance = TestPersistentSingleton.HasInstance;

            // Assert
            yield return null;
            Assert.IsFalse(hasInstance, "HasInstance should return false when no instance exists.");
        }

        [UnityTest]
        public IEnumerator TryGetInstance_ReturnsInstance_WhenInstanceExists()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestPersistentSingleton));
            var singleton = gameObject.AddComponent<TestPersistentSingleton>();

            // Act
            var instance = TestPersistentSingleton.TryGetInstance();

            // Assert
            yield return null;
            Assert.AreEqual(singleton, instance, "TryGetInstance should return the existing instance.");
        }

        [UnityTest]
        public IEnumerator TryGetInstance_ReturnsNull_WhenInstanceDoesNotExist()
        {
            // Act
            var instance = TestPersistentSingleton.TryGetInstance();

            // Assert
            yield return null;
            Assert.IsNull(instance, "TryGetInstance should return null when no instance exists.");
        }
    }

}