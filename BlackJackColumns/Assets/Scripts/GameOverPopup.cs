using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameOverPopup : MonoBehaviour
{
    private const string gameOverWinText = "You won the game";
    private const string gameOverTimeText = "You run out of time";
    private const string gameOverBustText = "You busted too many times";

    [SerializeField]
    private TMP_Text gameOverText;
    [SerializeField]
    private Button restartButton;

    private Action onRestartButtonPressed;

    public void OpenPopup(GameOverType type, Action onRestartButtonPressed)
    {
        this.onRestartButtonPressed = onRestartButtonPressed;
        restartButton.onClick.AddListener(RestartGame);
        SetGameOverText(type);
        gameObject.SetActive(true);
    }

    private void SetGameOverText(GameOverType type)
    {
        switch (type)
        {
            case GameOverType.Bust:
                gameOverText.text = gameOverBustText;
                break;
            case GameOverType.Time:
                gameOverText.text = gameOverTimeText;
                break;
            case GameOverType.Win:
                gameOverText.text = gameOverWinText;
                break;
            default:
                break;
        }
    }

    private void RestartGame()
    {
        onRestartButtonPressed?.Invoke();
        gameObject.SetActive(false);
        restartButton.onClick.RemoveAllListeners();
    }
}