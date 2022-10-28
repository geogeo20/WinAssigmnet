using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conifg", menuName = "ScriptableObjects/GameConfig", order = 1)]
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
}