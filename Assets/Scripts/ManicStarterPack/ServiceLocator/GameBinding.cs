using TowerDefense.Collection;
using TowerDefense.Manager;
using TowerDefense.WaveSystem;
using UnityEngine;

namespace Manic.Services
{
    [CreateAssetMenu(menuName = "GameBinding", fileName = "GameBinding")]
    public sealed class GameBinding : ScriptableObject
    {
        public WaveCollection WaveCollection => _waveCollection;
        [Header("Collections")]
        [SerializeField] private WaveCollection _waveCollection;
        public DefenderDataCollection DefenderDataCollection => _defenderDataCollection;
        [SerializeField] private DefenderDataCollection _defenderDataCollection;
        public RunnerDataCollection RunnerDataCollection => _runnerDataCollection;
        [SerializeField] private RunnerDataCollection _runnerDataCollection;
        
        public GameSceneManager GameSceneManager => _gameSceneManager;

        [Header("Managers")]
        [SerializeField] private GameSceneManager _gameSceneManager;
        public GameDataManager GameDataManager => _gameDataManager;
        [SerializeField] private GameDataManager _gameDataManager;
    }
}