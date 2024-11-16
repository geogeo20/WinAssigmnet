using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

public class GameOverPopup : MonoBehaviour
{
    [FormerlySerializedAs("gameOverText")] [SerializeField]
    private TMP_Text _infoText;
    [FormerlySerializedAs("restartButton")] [SerializeField]
    private Button _restartButton;

    private Action _onRestartButtonPressed;

    public void OpenPopup(GameOverType type, Action onRestartButtonPressed)
    {
        _onRestartButtonPressed = onRestartButtonPressed;
        _restartButton.onClick.AddListener(RestartGame);
        
        SetGameOverText(type);
        gameObject.SetActive(true);
    }

    private void SetGameOverText(GameOverType type)
    {
        var gameConfig = GameManager.Instance.GameConfig;
        
        _infoText.text = type switch
        {
            GameOverType.Bust => gameConfig.LOST_BUST_GAME_TEXT,
            GameOverType.Time => gameConfig.LOST_TIME_GAME_TEXT,
            GameOverType.Win => gameConfig.WON_GAME_TEXT,
            _ => _infoText.text
        };
    }

    private void RestartGame()
    {
        gameObject.SetActive(false);
        
        _onRestartButtonPressed?.Invoke();
        _onRestartButtonPressed = null;
        
        _restartButton.onClick.RemoveAllListeners();
    }
}