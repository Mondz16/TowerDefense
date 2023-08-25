using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manic.Services
{
    public static class Game
    {
        public static readonly ServiceLocator Services = new ServiceLocator();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            GameBinding binding = Resources.Load<GameBinding>("GameBinding");

            // example
            // Services.Add<SampleBind>(new UnityComponentServiceProvider<SampleBind>);

        }

        private static void InitializeUnityServices(GameBinding binding)
        {
            GameObject servicesObject = new GameObject("Services");
            Object.DontDestroyOnLoad(servicesObject);
            
        }
    }
}