using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Collection;
using TowerDefense.Manager;
using TowerDefense.PathFinding;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _waveIntervalText;
    [SerializeField] private TMP_Text _enemiesLeftText;
    [SerializeField] private TMP_Text _coinText;
    [SerializeField] private TMP_Text _playerHealthText;

    [Header("Post Game UI")] 
    [SerializeField]
    private GameObject _winPostGameHolder;
    
    [SerializeField] private List<DefenderDrag> _defenderDragList;

    public static Action<DefenderID> OnDefenderDropEvent { get; set; }
    public static Action OnPlayButtonClickedEvent { get; set; }
    public static Action OnRestartButtonClickedEvent { get; set; }

    private Timer _timer = null;
    
    private void OnEnable()
    {
        GameDataManager.OnPlayerHealthUpdatedEvent += UpdatePlayerHealthUI;
        GameDataManager.OnPlayerCoinsUpdatedEvent += UpdatePlayerCoinsUI;
        GameManager.OnNextWaveEvent += ShowNextWaveTimer;
        GameManager.OnWaveCompletedEvent += ShowWinPostGameUI;
    }

    private void OnDisable()
    {
        GameDataManager.OnPlayerHealthUpdatedEvent -= UpdatePlayerHealthUI;
        GameDataManager.OnPlayerCoinsUpdatedEvent -= UpdatePlayerCoinsUI;
        GameManager.OnNextWaveEvent -= ShowNextWaveTimer;
        GameManager.OnWaveCompletedEvent -= ShowWinPostGameUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (DefenderDrag defenderDrag in _defenderDragList)
            defenderDrag.InjectUIManager(this);
        
        _winPostGameHolder.SetActive(false);
    }

    private void Update()
    {
        if(_timer != null) _timer.Tick(Time.deltaTime);
    }

    private void ShowWinPostGameUI()
    {
        _winPostGameHolder.SetActive(true);
    }
    
    private void ShowNextWaveTimer(float interval, int wave)
    {
        _timer = new Timer(interval);
        _timer.OnSecondUpdate += TimerCountdown;
        _timer.OnTimerEnd += OnTimerEnds;
        _timer.OnTimerEnd += () => UpdateWave(wave);
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
            defenderDrag.UpdateDefenderState(coins);
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
        OnRestartButtonClickedEvent?.Invoke();
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
