using UnityEngine;

namespace DesignPattern.Singleton
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool AutoUnparrentOnAwake = true;
        protected static T instance;
        
        public static bool HasInstance => instance != null;
        public static T TryGetInstance() => HasInstance ? instance : null;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        var obj = new GameObject(typeof(T).Name + " Auto-Generated");
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        
        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        protected virtual void Awake() 
            => InitializeSingleton();

        private void InitializeSingleton()
        {
            // Return if application not running
            if (!Application.isPlaying) 
                return;
            
            // Set this game object parent to root
            if (AutoUnparrentOnAwake)
                transform.SetParent(null);

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) {
                DestroyImmediate(gameObject);
            }
        }
    }
}