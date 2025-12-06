using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ScriptableObject representing a card type with visual and identification data.
/// </summary>
[CreateAssetMenu(fileName = "New Card",menuName = "Card Game/Create New Card Type")]
public class CardData : ScriptableObject
{
    [Header("Card Visual")]
    [Tooltip("Sprite displayed on the front of the card.")]
    [SerializeField] private Sprite cardSprite;

    [Header("Card Identification")]
    [Tooltip("Unique identifier for this card type.")]
    public string ID;

    /// <summary>
    /// Public accessor for the card's front sprite.
    /// </summary>
    public Sprite CardSprite => cardSprite;
}