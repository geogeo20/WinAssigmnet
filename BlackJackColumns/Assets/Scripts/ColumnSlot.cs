using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class ColumnSlot : MonoBehaviour
{
    private const float inactiveScoreTextAlpha = .37f;

    [SerializeField]
    private Transform cardsHolder;
    [SerializeField]
    private TMP_Text scoreText;

    private int columnBustLimit;
    private int blackJackScore;

    private List<PlayCard> currentAttachedCards;
    private int bustsCounter;
    private int currentScore;
    private bool isBusted;
    private int acesCount;
    private Action<List<PlayCard>, bool> onColumnBust;

    private void Start()
    {
        currentAttachedCards = new();
        ResetScore();
    }

    public void Init(int columnBustLimit, int blackJackScore, Action<List<PlayCard>, bool> onColumnBust)
    {
        this.columnBustLimit = columnBustLimit;
        this.blackJackScore = blackJackScore;
        this.onColumnBust = onColumnBust;
    }

    public void ResetColumn()
    {
        ResetScore();
        bustsCounter = 0;
        isBusted = false;
        DropCards(false);
    }

    public bool CanAttach()
    {
        return !isBusted;
    }

    public void AttachCard(PlayCard obj)
    {
        obj.transform.SetParent(cardsHolder.transform);
        obj.transform.position = cardsHolder.position;
        currentAttachedCards.Add(obj);

        if (obj.WildCard)
        {
            DropCards(false);
            ResetScore();
        }
        else
        {
            if (obj.IsAce)
            {
                acesCount++;
            }
            UpdateScore(obj.CardValue);
        }
    }


    private void UpdateScore(int value)
    {
        currentScore += value;
        scoreText.text = currentScore.ToString();
        scoreText.alpha = 1;
        
        if (acesCount > 0)
        {
            int optimalScore = GetOptimalScore();
            if(optimalScore != currentScore)
            {
                scoreText.text = string.Concat(optimalScore, '/', currentScore);
            }
            CheckScore(optimalScore);
        }
        else
        {
            CheckScore(currentScore);
        }
    }

    private int GetOptimalScore()
    {
        int score = currentScore - 11 * acesCount;

        for (int i = 0; i < acesCount; i++)
        {
            if (score + 11 > blackJackScore)
            {
                score += 1;
            }
            else
            {
                score += 11;
            }
        }

        return score;
    }

    private void CheckScore(int score)
    {
        if (score >= blackJackScore)
        {
            if (score > blackJackScore)
            {
                bustsCounter++;
            }

            DropCards(score > blackJackScore);
            ResetScore();
            if (bustsCounter == columnBustLimit)
            {
                isBusted = true;
            }
        }
    }

    private IEnumerator InitDropCards(bool bust)
    {
        for (int i = currentAttachedCards.Count - 1; i >= 0 ; i--)
        {
            currentAttachedCards[i].DropCard();
            yield return new WaitForSeconds(.2f);
        }
        onColumnBust?.Invoke(currentAttachedCards, bust);
        currentAttachedCards.Clear();
    }

    private void DropCards(bool bust)
    {
        StartCoroutine(InitDropCards(bust));
    }

    private void ResetScore()
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();
        scoreText.alpha = inactiveScoreTextAlpha;
        acesCount = 0;
    }
}