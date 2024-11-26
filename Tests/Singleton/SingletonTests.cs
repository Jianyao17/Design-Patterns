using DesignPatterns.Singleton;
using NUnit.Framework;
using UnityEngine;

namespace DesignPatterns.Tests.Singleton
{
    public class SingletonTests
    {
        // Test class inheriting from Singleton for testing purposes
        public class TestSingleton : Singleton<TestSingleton>
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
            var existing = Object.FindObjectOfType<TestSingleton>();
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }
        }

        [Test]
        public void Awake_Calls_InitializeSingleton()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestSingleton));

            // Act
            var singleton = gameObject.AddComponent<TestSingleton>();

            // Assert
            Assert.IsTrue(singleton.AwakeCalled, "Awake should call InitializeSingleton.");
        }

        [Test]
        public void Instance_WhenNull_FindsExistingInstance()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestSingleton));
            var singleton = gameObject.AddComponent<TestSingleton>();

            // Act
            var instance = TestSingleton.Instance;

            // Assert
            Assert.AreEqual(singleton, instance, "Instance should return the existing instance.");
        }

        [Test]
        public void Instance_WhenNullAndNoExistingInstance_CreatesNewInstance()
        {
            // Act
            var instance = TestSingleton.Instance;

            // Assert
            Assert.IsNotNull(instance, "Instance should create a new instance if none exists.");
            Assert.IsTrue(instance.name.Contains(nameof(TestSingleton)), "The new instance should have the correct name.");
        }

        [Test]
        public void HasInstance_ReturnsTrue_WhenInstanceExists()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestSingleton));
            gameObject.AddComponent<TestSingleton>();

            // Act
            var hasInstance = TestSingleton.HasInstance;

            // Assert
            Assert.IsTrue(hasInstance, "HasInstance should return true when an instance exists.");
        }

        [Test]
        public void HasInstance_ReturnsFalse_WhenInstanceDoesNotExist()
        {
            // Act
            var hasInstance = TestSingleton.HasInstance;

            // Assert
            Assert.IsFalse(hasInstance, "HasInstance should return false when no instance exists.");
        }

        [Test]
        public void TryGetInstance_ReturnsInstance_WhenInstanceExists()
        {
            // Arrange
            var gameObject = new GameObject(nameof(TestSingleton));
            var singleton = gameObject.AddComponent<TestSingleton>();

            // Act
            var instance = TestSingleton.TryGetInstance();

            // Assert
            Assert.AreEqual(singleton, instance, "TryGetInstance should return the existing instance.");
        }

        [Test]
        public void TryGetInstance_ReturnsNull_WhenInstanceDoesNotExist()
        {
            // Act
            var instance = TestSingleton.TryGetInstance();

            // Assert
            Assert.IsNull(instance, "TryGetInstance should return null when no instance exists.");
        }
    }
}