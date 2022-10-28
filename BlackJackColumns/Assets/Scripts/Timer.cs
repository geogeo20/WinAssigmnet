using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    private int totalTime;
    private Action onCountdownComplete;

    public void InitTimer(int value, Action onCountdownComplete)
    {
        totalTime = value;
        timerText.text = "00:00";
        this.onCountdownComplete = onCountdownComplete;
    }

    public void StopTimer()
    {
        StopCoroutine(CountdownTimer());
    }

    public void StarTimer()
    {
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        int timeRemaining = totalTime;

        while (timeRemaining >= 0)
        {
            string minutes = Mathf.Floor(timeRemaining / 60).ToString("00");
            string seconds = (timeRemaining % 60).ToString("00");

            timerText.text = string.Format("{0}:{1}", minutes, seconds);
            yield return new WaitForSeconds(1);
            timeRemaining -= 1;
        }

        onCountdownComplete?.Invoke();
    }
}
