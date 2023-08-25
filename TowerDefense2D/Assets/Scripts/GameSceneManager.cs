using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Manager
{
    public class GameSceneManager : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}