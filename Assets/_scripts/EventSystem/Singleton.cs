using System;
using UnityEngine;

namespace GellosGames
{

    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        protected bool persistent = true;
        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton, System.Threading.LazyThreadSafetyMode.PublicationOnly);
        public static T Instance => LazyInstance.Value;

        private static T CreateSingleton()
        {
            T instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                instance = singletonObject.AddComponent<T>();
            }
            return instance;
        }

        public virtual void Awake()
        {
            if (persistent) DontDestroyOnLoad(gameObject);
            if (Instance != this) { Destroy(gameObject); }
        }
    } // https://csharpindepth.com/Articles/Singleton
}