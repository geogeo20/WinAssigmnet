using System;
using UnityEngine;

/// <summary>
/// Structure holding information about a specific card
/// </summary>
[Serializable]
public class CardData
{
    public Sprite cardVisual;
    public int cardValue;
    public bool isWildCard;
    public bool isAce;
}