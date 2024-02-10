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
    }

    [Serializable]
    public class DefenderData
    {
        public DefenderID ID;
        public string DefenderName;
        public int Health;
        public int Damage;
        public float Range;
        public float AttackSpeed;
        public BulletController BulletPrefab;
    }

    public enum DefenderID
    {
        Watermelon, Pineapple , Tomato , Carrot , Pumpkin 
    }
}