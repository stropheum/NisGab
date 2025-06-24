using UnityEngine;

namespace NisGab
{
    /// <summary>
    /// Singleton that initializes once accessed. Does not require a scene instance
    /// </summary>
    /// <typeparam name="T">The type of singleton component</typeparam>
    public class LazySingleton<T> : PersistentSingleton<T> where T : LazySingleton<T>
    {      
        public new static T Instance
        {
            get
            {
                if (_instance != null) { return _instance; }

                var obj = new GameObject("<Singleton> " + typeof(T).Name);
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }
    }
}