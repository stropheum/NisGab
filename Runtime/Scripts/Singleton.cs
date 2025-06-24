using UnityEngine;

namespace NisGab
{
    /// <summary>
    /// Singleton that requires initialization 
    /// </summary>
    /// <typeparam name="T">The type of singleton component</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) { return _instance; }

                Debug.LogError(typeof(Singleton<T>).Name + " is not initialized");
                return null;
            }
        }
        
        public static bool IsValid() => _instance != null;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log("Duplicate Singleton Instance<" + typeof(T).Name + ">. Destroying...");
                Destroy(this);
            }
            else
            {
                _instance = this as T;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (gameObject != null)
            {
                DestroyImmediate(gameObject);   
            }
        }
    }
}