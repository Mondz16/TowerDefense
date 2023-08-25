using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.WaveSystem
{
    [CreateAssetMenu(menuName = "WaveCollection", fileName = "WaveCollection")]
    public class WaveCollection : ScriptableObject
    {
        public List<WaveVariation> WaveList;
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