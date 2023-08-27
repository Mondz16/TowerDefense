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

        public List<Wave> GetWaveByLevel(WaveLevel level)
        {
            return _waveList.Find(x => x.Level == level).LevelWaveList;
        }

        public List<WaveVariation> GetWaveVariation()
        {
            return _waveList;
        }
    }
    
    [Serializable]
    public class WaveVariation
    {
        public WaveLevel Level;
        public List<Wave> LevelWaveList;
    }

    public enum WaveLevel
    {
        Level1, Level2,Level3,Level4,Level5
    }
}