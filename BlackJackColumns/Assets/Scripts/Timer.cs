using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    private float _totalTime;
    private float _currentTIme;
    
    private bool _canStart;
    
    private Action _onCountdownComplete;

    public void InitTimer(int totalTime, Action onCountdownComplete)
    {
        _totalTime = totalTime;
        _timerText.text = "00:00";
        _onCountdownComplete = onCountdownComplete;
    }

    public void Start()
    {
        _canStart = true;
        _currentTIme = _totalTime;
    }

    private void Update()
    {
        if (_canStart == false)
            return;
        
        _currentTIme -= Time.deltaTime;
        var timeSpan = TimeSpan.FromSeconds(_currentTIme);
        _timerText.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

        if (_currentTIme >= 0)
            return;
        
        _canStart = false;
        _onCountdownComplete.Invoke();
    }
}
