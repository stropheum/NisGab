namespace NisGab
{
    /// <summary>
    /// Specialized type of singleton that will persist across scene loads
    /// </summary>
    /// <typeparam name="T"> The wrapper type of the singleton instance </typeparam>
    public class PersistentSingleton<T> : Singleton<T> where T : PersistentSingleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}