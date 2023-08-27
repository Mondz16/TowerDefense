using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Collection;
using TowerDefense.PathFinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TowerDefense.WaveSystem
{
    [CreateAssetMenu(menuName = "New Wave", fileName = "Wave")]
    public class Wave : ScriptableObject
    {
        public float TimeInterval => _timeInterval;
        
        [SerializeField] private GameDifficulty _difficulty;
        [SerializeField] private float _timeInterval;
        [SerializeField] private List<WaveAttribute> _waveAttributeList;

        public List<WaveAttribute> GetWaveAttributeList()
        {
            return _waveAttributeList;
        }

        public WaveAttribute GetWaveAttributeListByRandom()
        {
            return _waveAttributeList[Random.Range(0, _waveAttributeList.Count)];
        }
    }

    [Serializable]
    public class WaveAttribute
    {
        public RunnerID RunnerID;
        public int EnemyCount;
    }
}