using System.Collections;
using System.Collections.Generic;
using TowerDefense.PathFinding;
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
            InitializeUnityServices(binding);

            // example
            // Services.Add<SampleBind>(new UnityComponentServiceProvider<SampleBind>());
            Services.Add<PoolManager>(new UnityComponentServiceProvider<PoolManager>());
            Services.Add<MapManager>(new UnityComponentServiceProvider<MapManager>());

            Services.Add(binding.WaveCollection);
            Services.Add(binding.DefenderDataCollection);
            Services.Add(binding.RunnerDataCollection);
        }

        private static void InitializeUnityServices(GameBinding binding)
        {
            GameObject servicesObject = new GameObject("Services");
            Object.DontDestroyOnLoad(servicesObject);

            InitializeManager(servicesObject, binding.GameSceneManager);
            InitializeManager(servicesObject, binding.GameDataManager);
        }

        private static void InitializeManager<T>(GameObject servicesObject, T binding) where T : Object
        {
            var manager = Object.Instantiate(binding, servicesObject.transform);
            Services.Add(manager);
        }
    }
}