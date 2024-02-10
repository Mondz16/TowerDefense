using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TowerDefense.Manager;
using TowerDefense.PathFinding;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _waveIntervalText;
    [SerializeField] private TMP_Text _enemiesLeftText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _playerHealthText;
    [SerializeField] private List<DefenderDrag> _defenderDragList;

    public static Action OnDefenderDropEvent { get; set; }
    public static Action OnPlayButtonClickedEvent { get; set; }

    private Timer _timer = null;
    
    private void OnEnable()
    {
        GameDataManager.OnPlayerHealthUpdatedEvent += UpdatePlayerHealthUI;
        GameManager.OnNextWaveEvent += ShowNextWaveTimer;
    }

    private void OnDisable()
    {
        GameDataManager.OnPlayerHealthUpdatedEvent -= UpdatePlayerHealthUI;
        GameManager.OnNextWaveEvent -= ShowNextWaveTimer;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (DefenderDrag defenderDrag in _defenderDragList)
            defenderDrag.InjectUIManager(this);
    }

    private void Update()
    {
        if(_timer != null) _timer.Tick(Time.deltaTime);
    }

    private void ShowNextWaveTimer(float interval)
    {
        _timer = new Timer(interval);
        _timer.OnSecondUpdate += TimerCountdown;
        _timer.OnTimerEnd += OnTimerEnds;
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

    private void UpdatePlayerHealthUI(int health)
    {
        _playerHealthText.text = health.ToString();
    }

    public void OnDefenderDropEventTriggered()
    {
        OnDefenderDropEvent?.Invoke();
    }

    public void OnPlayButtonClickedEventTrigger()
    {
        OnPlayButtonClickedEvent?.Invoke();
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
