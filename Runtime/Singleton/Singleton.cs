using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
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
            if (!Application.isPlaying) return;
            instance = this as T;
        }
    }
}
