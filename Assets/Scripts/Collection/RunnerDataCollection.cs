using System;
using System.Collections;
using System.Collections.Generic;
using Manic.Services;
using TowerDefense.PathFinding;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense.Collection
{
    public class RunnerDataCollection : ScriptableObject
    {
        public static RunnerDataCollection Service
        {
            get
            {
                if (_ == null)
                    _ = Game.Services.Get<RunnerDataCollection>();

                return _;
            }
        }

        private static RunnerDataCollection _;
        
        [SerializeField] private List<RunnerData> _runnerDataList;

        public RunnerData GetRunnerDataByID(RunnerID id)
        {
            return _runnerDataList.Find(x => x.ID == id);
        }
    }

    [Serializable]
    public class RunnerData
    {
        public RunnerID ID;
        public string RunnerName;
        public int Health;
        public float Speed;
        public string Description;
        public GameObject Prefab;

        public RunnerData(RunnerData runnerData)
        {
            ID = runnerData.ID;
            RunnerName = runnerData.RunnerName;
            Health = runnerData.Health;
            Speed = runnerData.Speed;
            Description = runnerData.Description;
            Prefab = runnerData.Prefab;
        }
    }

    public enum RunnerID
    {
        Hamburger, Fries , Pasta , IceCream, Donut , Pizza
    }
}