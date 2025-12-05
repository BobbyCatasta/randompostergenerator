using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card",menuName = "CardGame/Create New Card Type")]
public class CardData : ScriptableObject
{
    /// <summary>
    /// Image that will be displayed on the card when assigning this scriptable to CardBehaviour.
    /// </summary>
    [SerializeField] private Sprite cardImage;

    /// <summary>
    /// Sprite of the card that will be displayed.
    /// </summary>
    public Sprite CardImage => cardImage;
}
