using UnityEngine;

namespace NISGAB
{
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

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError("Duplicate Singleton Instance. Destroying...");
                Destroy(this);
            }
        }
    }
}