using System;
using System.Collections;
using System.Collections.Generic;
using Manic.Services;
using UnityEngine;

namespace TowerDefense.WaveSystem
{
    [CreateAssetMenu(menuName = "WaveCollection", fileName = "WaveCollection")]
    public class WaveCollection : ScriptableObject
    {
        public static WaveCollection Service
        {
            get
            {
                if (_ == null)
                    _ = Game.Services.Get<WaveCollection>();

                return _;
            }
        }

        private static WaveCollection _;
        
        [SerializeField] private List<WaveVariation> _waveList;

        public List<Wave> GetWaveByDifficulty(Difficulty difficulty)
        {
            return _waveList.Find(x => x.Difficulty == difficulty).LevelWaveList;
        }

        public List<WaveVariation> GetWaveVariation()
        {
            return _waveList;
        }
    }
    
    [Serializable]
    public class WaveVariation
    {
        public Difficulty Difficulty;
        public List<Wave> LevelWaveList;
    }

    public enum Difficulty
    {
        Easy, Normal, Hard, Extreme
    }
}