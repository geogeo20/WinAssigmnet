using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Game config")]
    [SerializeField]
    private GameConfig gameConfig;
    public GameConfig GameConfig { get { return gameConfig; } }

    [Space]
    [SerializeField]
    private PlayCard playCardReference;
    [SerializeField]
    private Transform cardsHolder;
    [SerializeField]
    private Timer timer;
    [SerializeField]
    private List<ColumnSlot> columnSlots;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private GameOverPopup gameOverPopup;
    [SerializeField]
    private Transform slotPosition;

    private PoolList<PlayCard> deckPool;
    private PoolList<PlayCard> columnPool;
    private List<CardData> availableCards;
    private int bustCounter;

    private int cardsInDeck;

    private void Start()
    {
        availableCards = new(GameConfig.CardsConfig);
        deckPool = new();
        deckPool.Init(playCardReference, cardsHolder);
        playButton.onClick.AddListener(StartGame);
        InitColumns();
        InitTimer();
    }

    private void InitColumns()
    {
        foreach (var column in columnSlots)
        {
            column.Init(GameConfig.ColumnBustLimit, GameConfig.BlackJackScore, DropCards);
        }

    }

    private void InitTimer()
    {
        timer.InitTimer(GameConfig.TimeLimit, CountdownComplete);
    }

    private void GenerateInitialDeck()
    {
        for (int i = 0; i < GameConfig.MaxCardsInDeck; i++)
        {
            cardsInDeck++;
            GenerateCard();
        }
        deckPool.ItemsDisplayed[0].MoveToCurrentSlot();

    }

    private void CountdownComplete()
    {
        GameOver(GameOverType.Time);
    }

    private void GameOver(GameOverType type)
    {
        gameOverPopup.OpenPopup(type, ResetGame);
    }

    private void StartGame()
    {
        timer.StarTimer();
        GenerateInitialDeck();
        playButton.gameObject.SetActive(false);
    }

    private void ResetGame()
    {
        bustCounter = 0;
        playButton.gameObject.SetActive(true);
        availableCards.Clear();
        availableCards = new(GameConfig.CardsConfig);
        foreach (var item in columnSlots)
        {
            item.ResetColumn();
        }
        deckPool.DeleteAll();
        cardsInDeck = 0;
    }
    
    public void GenerateCard()
    {
        if (availableCards.Count == 0)
        {
            cardsInDeck--;
            if (cardsInDeck == 0)
            {
                GameOver(GameOverType.Win);
            }
            return;
        }

        int index = UnityEngine.Random.Range(0, availableCards.Count);

        foreach (var item in deckPool.ItemsDisplayed)
        {
            item.MoveCard();
        }

        PlayCard instance = deckPool.CreateItem();
        instance.Init(availableCards[index], AttachCard, slotPosition);
        availableCards.RemoveAt(index);
    }

    private void AttachCard(PlayCard card)
    {
        deckPool.RemoveFromPool(card);
        GenerateCard();
        if(deckPool.ItemsDisplayed.Count != 0)
        {
            deckPool.ItemsDisplayed[0].MoveToCurrentSlot();
        }
    }

    public void DropCards(List<PlayCard> cards, bool bust = false)
    {
        deckPool.AddToPool(cards);

        if (bust)
        {
            bustCounter++;
        }

        if(bustCounter == GameConfig.TotalBustLimit)
        {
            GameOver(GameOverType.Bust);
        }
    }

    protected override void OnDestroy()
    {
        playButton.onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}