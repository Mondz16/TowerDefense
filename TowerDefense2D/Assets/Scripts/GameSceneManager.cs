using Manic.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Manager
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Service
        {
            get
            {
                if (_)
                    _ = Game.Services.Get<GameSceneManager>();

                return _;
            }
        }

        private static GameSceneManager _;
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}