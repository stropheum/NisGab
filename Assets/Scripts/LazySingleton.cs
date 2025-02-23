using UnityEngine;

namespace NISGAB
{
    /// <summary>
    /// Singleton that initializes once accessed. Does not require 
    /// </summary>
    /// <typeparam name="T">The type of singleton component</typeparam>
    public class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("<Singleton> " + typeof(T).Name);
                    _instance = obj.AddComponent<T>();    
                }
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError("Duplicate Singleton Instance. Destroying...");
                Destroy(this);
            }

            _instance = this as T;
        }
    }
}