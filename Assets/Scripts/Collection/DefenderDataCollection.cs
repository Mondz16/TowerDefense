using System;
using System.Collections;
using System.Collections.Generic;
using Manic.Services;
using UnityEngine;

namespace TowerDefense.Collection
{
    public class DefenderDataCollection : ScriptableObject
    {
        public static DefenderDataCollection Service
        {
            get
            {
                if (_ == null)
                    _ = Game.Services.Get<DefenderDataCollection>();

                return _;
            }
        }

        private static DefenderDataCollection _;
        
        [SerializeField] private List<DefenderData> _defenderDataList;

        public DefenderData GetDefenderDataByID(DefenderID id)
        {
            return _defenderDataList.Find(x => x.ID == id);
        }

        public DefenderStats GetDefenderStatsByLevel(DefenderID id, int level)
        {
            var defenderData = GetDefenderDataByID(id);
            return defenderData.DefenderStatsList.Find(x => x.Level == level);
        }
    }

    [Serializable]
    public class DefenderData
    {
        public DefenderID ID;
        public string DefenderName;
        public List<DefenderStats> DefenderStatsList;
        public ShooterController DefenderController;
        public BulletController BulletPrefab;
    }

    [Serializable]
    public class DefenderStats
    {
        public int Level;
        public int Health;
        public int Damage;
        public int Cost;
        public float Range;
        public float AttackSpeed;
        public Sprite DefenderSprite;
        public Sprite BulletSprite;
    }

    public enum DefenderID
    {
        Watermelon, Pineapple , Tomato , Carrot , Grape 
    }
}