using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
#endif

namespace GellosGames
{
	public class Locator : MonoBehaviour
	{

#if UNITY_EDITOR
        static Locator instance;
        // Only for Test Reasons
        [Header("default Profil Names")]
        public List<string> LoadedServices;
#endif
        static readonly Dictionary<Type, IService> serviceContainer = new Dictionary<Type, IService>();

        void Awake()
        {
#if UNITY_EDITOR
            instance = this;
#endif
            Profile.LoadRessorces();

        }

        public static void AddService(IService service)
        {
            serviceContainer.Add(service.GetType(), service);
        }
        public static void ProvideService(IService service)
        {
            serviceContainer[service.GetType()] = service;
        }
        internal static void RemoveService<T>()
        {
            serviceContainer.Remove(typeof(T));
        }
        public static T GetService<T>()
        {
            return (T)serviceContainer[typeof(T)];
        }

#if UNITY_EDITOR
        public static void SetCurrentProfile<T>(string profileFileName, T service) where T : Profile
        {
            for (int i = 0; i < instance.LoadedServices.Count; i++)
			{
               if(instance.LoadedServices[i].Contains(typeof(T).Name))
				{
                    instance.LoadedServices[i] = SettedProfile();
                    return;
			    }
            }
            instance.LoadedServices.Add(SettedProfile());
            string SettedProfile() => typeof(T).Name + ": " + profileFileName;

        }
#endif
    }
    public interface IService
    {

    }

}
