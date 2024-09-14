using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Manager;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _canvasObject;
    [SerializeField] private Slider _progressBarSlider;
    [SerializeField] private float _loadTime;

    private float _progressValue = 0f;
    private float _targetProgressValue = 0f;
    
    private void OnEnable()
    {
        SceneLoaderManager.OnLoadSceneEvent += UpdateProgressBar;
    }

    private void OnDisable()
    {
        SceneLoaderManager.OnLoadSceneEvent -= UpdateProgressBar;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(_progressValue - _targetProgressValue) > 0.001f)
        {
            _progressValue = Mathf.Lerp(_progressValue, _targetProgressValue, Time.deltaTime * _loadTime);
            _progressBarSlider.value = _progressValue;
        }
    }

    private void UpdateProgressBar()
    {
        if (!_canvasObject.activeInHierarchy)
        {
            _canvasObject.SetActive(true);
            _targetProgressValue = 1.1f;
        }
    }
}
