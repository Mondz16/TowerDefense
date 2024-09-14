using System;
using System.Collections;
using System.Collections.Generic;
using Manic.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Manager
{
    public class SceneLoaderManager : MonoBehaviour
    {
        public static SceneLoaderManager Services
        {
            get
            {
                if (_ == null)
                    _ = Game.Services.Get<SceneLoaderManager>();
                return _;
            }
        }

        private static SceneLoaderManager _;

        public float Delay = 5f;
        public static Action OnLoadSceneEvent { get; set; }

        public void SimpleLoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneWithLoadingScreen(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            OnLoadSceneEvent?.Invoke();
            yield return new WaitForSeconds(Delay);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        } 
    }
}