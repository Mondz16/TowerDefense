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
        public int CoinWaveReward => _coinWaveReward;
        
        [SerializeField] private GameDifficulty _difficulty;
        [SerializeField] private float _timeInterval;
        [SerializeField] private int _coinWaveReward;
        [SerializeField] private List<WaveAttribute> _waveAttributeList;

        private RunnerID[] _runnerIds = new []{RunnerID.Hamburger, RunnerID.Fries, RunnerID.PizzaPiece, RunnerID.Soda, RunnerID.Donut, RunnerID.Pizza};        

        public List<WaveAttribute> GetWaveAttributeList()
        {
            return _waveAttributeList;
        }

        public List<WaveAttribute> GetAllEnemyWaveCount()
        {
            List<WaveAttribute> waveAttributeList = new List<WaveAttribute>();
            foreach (RunnerID runnerId in _runnerIds)
            {
                var runnerList = _waveAttributeList.FindAll(x => x.RunnerID == runnerId);
                int count = 0;
                foreach (WaveAttribute runnerAttribute in runnerList)
                    count += runnerAttribute.EnemyCount;

                if(count != 0)
                    waveAttributeList.Add(new WaveAttribute(runnerId, count));
            }

            return waveAttributeList;
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

        public WaveAttribute(){}

        public WaveAttribute(RunnerID id, int enemyCount)
        {
            RunnerID = id;
            EnemyCount = enemyCount;
        }
    }
}