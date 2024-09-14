using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Collection;
using TowerDefense.PathFinding;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    public class EnemyCounterUI : MonoBehaviour
    {
        [SerializeField] private Image _enemySprite;
        [SerializeField] private TMP_Text _enemyCountText;

        private RunnerID _runnerID;
        private int _count;

        private void OnEnable()
        {
            GameManager.OnSpawnRunnerEvent += UpdateEnemyCounter;
        }

        private void OnDisable()
        {
            GameManager.OnSpawnRunnerEvent -= UpdateEnemyCounter;
        }

        public void SetEnemyCounterUI(Sprite enemySprite, int count, RunnerID id)
        {
            _runnerID = id;
            _count = count;
            _enemySprite.sprite = enemySprite;
            _enemyCountText.text = count.ToString();
        }

        private void UpdateEnemyCounter(RunnerID id)
        {
            if (_runnerID == id)
            {
                _count--;
                _enemyCountText.text = _count.ToString();
            }
        }
    }
}