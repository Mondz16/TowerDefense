using System.Collections;
using System.Collections.Generic;
using ManicStarterPack.Utilties;
using UnityEngine;

namespace TowerDefense.Manager
{
    public class MenuUIManager : MonoBehaviour
    {
        private SceneLoaderManager _sceneLoaderManager => SceneLoaderManager.Services;
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void PlayGameButtonClicked()
        {
            _sceneLoaderManager.LoadSceneWithLoadingScreen(Constants.SceneName_Game);
        }
    }
}