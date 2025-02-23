using UnityEngine;

namespace NISGAB
{
    public class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
    {
        protected static T _instance;
    }
}