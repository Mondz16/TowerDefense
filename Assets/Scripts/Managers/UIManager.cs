using System;
using System.Collections;
using System.Collections.Generic;
using ManicStarterPack.Utilties;
using TMPro;
using TowerDefense.Collection;
using TowerDefense.Manager;
using TowerDefense.PathFinding;
using TowerDefense.UI;
using TowerDefense.WaveSystem;
using UnityEngine;

namespace TowerDefense.Manager
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Text")] [SerializeField] private TMP_Text _waveText;
        [SerializeField] private TMP_Text _waveIntervalText;
        [SerializeField] private TMP_Text _enemiesLeftText;
        [SerializeField] private TMP_Text _coinText;
        [SerializeField] private TMP_Text _playerHealthText;
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private EnemyWaveCounterManager _enemyWaveCounterManager;
        [SerializeField] private DefenderStatsIndicatorUI _defenderStatsIndicatorUI;

        [Header("Post Game UI")] 
        [SerializeField] private GameObject _winPostGameHolder;
        [SerializeField] private GameObject _losePostGameHolder;

        [SerializeField] private List<DefenderDrag> _defenderDragList;
        private bool _isWaveStarted = false;
        
        public static Action<DefenderID> OnDefenderDropEvent { get; set; }
        public static Action OnPlayButtonClickedEvent { get; set; }
        public static Action OnRestartButtonClickedEvent { get; set; }
        public static Action OnUpgradeDefenderEvent { get; set; }
        public static Action OnSellDefenderEvent { get; set; }

        private Timer _timer = null;

        private void OnEnable()
        {
            GameDataManager.OnPlayerHealthUpdatedEvent += UpdatePlayerHealthUI;
            GameDataManager.OnPlayerDiedEvent += ShowLosePostGameUI;
            GameDataManager.OnPlayerCoinsUpdatedEvent += UpdatePlayerCoinsUI;
            GameManager.OnNextWaveEvent += ShowNextWaveTimer;
            GameManager.OnStartWaveEvent += OnStartWaveStarted;
            GameManager.OnWaveCompletedEvent += ShowWinPostGameUI;
            GameManager.OnShowDefenderStatsEvent += ShowDefenderStatsUI;
            GameManager.OnPlayerTouchFieldEvent += HideDefenderStatsUI;
        }

        private void OnDisable()
        {
            GameDataManager.OnPlayerHealthUpdatedEvent -= UpdatePlayerHealthUI;
            GameDataManager.OnPlayerDiedEvent -= ShowLosePostGameUI;
            GameDataManager.OnPlayerCoinsUpdatedEvent -= UpdatePlayerCoinsUI;
            GameManager.OnNextWaveEvent -= ShowNextWaveTimer;
            GameManager.OnStartWaveEvent -= OnStartWaveStarted;
            GameManager.OnWaveCompletedEvent -= ShowWinPostGameUI;
            GameManager.OnShowDefenderStatsEvent -= ShowDefenderStatsUI;
            GameManager.OnPlayerTouchFieldEvent -= HideDefenderStatsUI;
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (DefenderDrag defenderDrag in _defenderDragList)
                defenderDrag.InjectUIManager(this);

            _winPostGameHolder.SetActive(false);
            _losePostGameHolder.SetActive(false);
        }

        private void Update()
        {
            if (_timer != null) _timer.Tick(Time.deltaTime);
        }

        private void ShowDefenderStatsUI(Vector3 pos, bool isUpgradeable)
        {
            _defenderStatsIndicatorUI.InjectUIManager(this);
            _defenderStatsIndicatorUI.gameObject.SetActive(true);
            var position = Helper.GetRectTransformOfWorldPosition(pos, _canvasRectTransform);
            _defenderStatsIndicatorUI.GetComponent<RectTransform>().anchoredPosition = position;
            _defenderStatsIndicatorUI.SetDefenderStatsUI(isUpgradeable);
        }

        private void HideDefenderStatsUI()
        {
            _defenderStatsIndicatorUI.gameObject.SetActive(false);
        }

        private void ShowWinPostGameUI()
        {
            _winPostGameHolder.SetActive(true);
        }

        private void ShowLosePostGameUI()
        {
            _losePostGameHolder.SetActive(true);
        }

        private void OnStartWaveStarted(Wave wave)
        {
            _enemyWaveCounterManager.SetCurrentEnemyWave(wave);

            _isWaveStarted = true;
        }
        
        private void ShowNextWaveTimer(float interval, int wave)
        {
            _timer = new Timer(interval);
            _timer.OnSecondUpdate += TimerCountdown;
            _timer.OnTimerEnd += OnTimerEnds;
            _timer.OnTimerEnd += () => UpdateWave(wave);
            
            _isWaveStarted = false;
        }
        
        private void TimerCountdown(float seconds)
        {
            _waveIntervalText.text = $"Next Wave: {seconds.ToString("0")}s";
        }

        private void OnTimerEnds()
        {
            _waveIntervalText.text = String.Empty;
            _timer = null;
        }

        private void UpdateWave(int wave)
        {
            _waveText.text = $"Wave {wave + 1}";
        }

        private void UpdatePlayerHealthUI(int health)
        {
            _playerHealthText.text = health.ToString();
        }

        private void UpdatePlayerCoinsUI(int coins)
        {
            _coinText.text = coins.ToString();
            foreach (DefenderDrag defenderDrag in _defenderDragList)
                defenderDrag.UpdateDefenderState(_isWaveStarted, coins);
        }

        public void OnUpgradeDefenderEventTriggered()
        {
            HideDefenderStatsUI();
            OnUpgradeDefenderEvent?.Invoke();
        }

        public void OnSellDefenderEventTriggered()
        {
            HideDefenderStatsUI();
            OnSellDefenderEvent?.Invoke();
        }
        
        public void OnDefenderDropEventTriggered(DefenderID defenderID)
        {
            OnDefenderDropEvent?.Invoke(defenderID);
        }

        public void OnPlayButtonClickedEventTrigger()
        {
            OnPlayButtonClickedEvent?.Invoke();
        }

        public void OnRestartButtonClickedEventTrigger()
        {
            SceneLoaderManager.Services.LoadSceneWithLoadingScreen(Constants.SceneName_Game);
            OnRestartButtonClickedEvent?.Invoke();
        }

        public void OnMenuButtonClickedEventTrigger()
        {
            SceneLoaderManager.Services.LoadSceneWithLoadingScreen(Constants.SceneName_Menu);
            
        }

        public void OnPauseButtonClicked()
        {
            Time.timeScale = 0;
        }

        public void OnFastForwardButtonClicked()
        {
            Time.timeScale = 2;
        }

        public void OnNormalSpeedButtonClicked()
        {
            Time.timeScale = 1;
        }
    }
}