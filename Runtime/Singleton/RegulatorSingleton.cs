using System.Linq;
using UnityEngine;

namespace DesignPattern.Singleton
{
    /// <summary>
    /// Persistent Regulator singleton, will destroy any other older components of the same type it finds on awake
    /// </summary>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        public float InitilizationTime { get; private set; }
        
        public static bool HasInstance => instance != null;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        var obj = new GameObject(typeof(T).Name + " Auto-Generated");
                        
                        // Prevent obj to be saved in scene
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        
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
            if (!Application.isPlaying) return;
            
            InitilizationTime = Time.time;
            DontDestroyOnLoad(gameObject);
            
            // Destroy all any old singleton with the same type
            T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (var oldInstance in oldInstances)
            {
                if (oldInstance.GetComponent<RegulatorSingleton<T>>().InitilizationTime < InitilizationTime)
                    Destroy(oldInstance.gameObject);
            }
            
            if (instance == null) {
                instance = this as T;
            }
        }
    }
}