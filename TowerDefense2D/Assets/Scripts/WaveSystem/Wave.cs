using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.WaveSystem
{
    [CreateAssetMenu(menuName = "New Wave", fileName = "Wave")]
    public class Wave : ScriptableObject
    {
        public GameDifficulty Difficulty;
        public float TimeInterval;
        public List<WaveAttribute> WaveAttributeList;
    }

    [Serializable]
    public class WaveAttribute
    {
        public GameObject Runner;
        public int EnemyCount;
    }
}