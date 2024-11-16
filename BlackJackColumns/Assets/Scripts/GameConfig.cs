using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public List<CardData> CardsConfig;

    public Sprite CardBackVisual;

    public int ColumnBustLimit;

    public int TotalBustLimit;

    public int TimeLimit;

    public int WildCardsCount;

    public int BlackJackScore;

    public int MaxCardsInDeck;

    [Header("Localization")] 
    
    public string WON_GAME_TEXT = "You won the game";
    public string LOST_TIME_GAME_TEXT = "You run out of time";
    public string LOST_BUST_GAME_TEXT = "Busted too many  times";
    public string BLACKJACK = "Blackjack";
    public string BUST = "Bust";
}