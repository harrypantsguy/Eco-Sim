using UnityEngine;

namespace CustomUI
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Singleton { get; private set; }

        private void Awake()
        {
            Singleton = this as T;
        }
    }
}