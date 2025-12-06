using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour component representing a card in the matching game.
/// </summary>
[RequireComponent(typeof(Image), typeof(Outline))]
public class CardBehaviour : MonoBehaviour, I_Card,IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    [Header("Card Visuals")]
    [Tooltip("Sprite displayed on the back of the card.")]
    [SerializeField] private Sprite cardBack;

    [Tooltip("Sprite displayed when in Blind Mode, instead of the real suit.")]
    [SerializeField] private Sprite questionMarkFront;

    /// <summary>
    /// Outline component for visual feedback.
    /// </summary>
    private Outline outlineComponent;
    
    /// <summary>
    /// Image component for displaying card sprites.
    /// </summary>
    private Image imageComponent;

    /// <summary>
    /// Data associated with this card.
    /// </summary>
    private CardData cardData;

    /// <summary>
    /// Indicates if the card can be interacted with.
    /// </summary>
    private bool isInteractable = true;

    /// <summary>
    /// Indicates if the card has been revealead at least once. (For Blind Mode Only!)
    /// </summary>
    private bool hasBeenRevealedOnce = false;

    /// <summary>
    /// Public accessor for card data.
    /// </summary>
    public CardData CardData => cardData;

    /// <summary>
    /// Indicates if the card is currently selected.
    /// </summary>
    public bool IsSelected { get; private set; }
    
    /// <summary>
    /// Indicates if the card is flipped (showing front).
    /// </summary>
    public bool IsFlipped { get; private set; }
    
    /// <summary>
    /// Indicates if the card has been matched.
    /// </summary>
    public bool IsMatched { get; private set; }
    
    /// <summary>
    /// Indicates if the card is interactable.
    /// </summary>
    public bool IsInteractable => isInteractable;
    
    /// <summary>
    /// Indicates if the card was flipped by player action.
    /// </summary>
    public bool IsFlippedByPlayer { get; private set; }

    /// <summary>
    /// Unique identifier from card data.
    /// </summary>
    public string ID => CardData.ID;

    /// <summary>
    /// Initializes component references.
    /// </summary>
    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        outlineComponent = GetComponent<Outline>();
        imageComponent.preserveAspect = true;
    }

    /// <summary>
    /// Shows the front (face) of the card.
    /// </summary>
    /// <param name="flippedByPlayer">Whether flipped by player action.</param>
    public void ShowFront(bool flippedByPlayer)
    {
        if(hasBeenRevealedOnce && GameBoot.IsBlindMode)
            imageComponent.sprite = questionMarkFront;
        else
            imageComponent.sprite = cardData.CardSprite;
        outlineComponent.effectColor = Color.black;
        IsFlipped = true;
        if (flippedByPlayer)
        {
            hasBeenRevealedOnce = true;
            IsFlippedByPlayer = true;
        }
    }

    /// <summary>
    /// Shows the back (hidden) of the card.
    /// </summary>
    public void ShowBack()
    {
        imageComponent.sprite = cardBack;
        outlineComponent.effectColor = Color.black;
        IsFlipped = false;
        IsFlippedByPlayer = false;
    }

    /// <summary>
    /// Disables interaction with the card.
    /// </summary>
    public void DisableCard()
    {
        isInteractable = false;
    }

    /// <summary>
    /// Enables interaction with the card.
    /// </summary>
    public void EnableCard()
    {
        isInteractable = true;
    }

    /// <summary>
    /// Initializes the card with specific data.
    /// </summary>
    /// <param name="data">CardData to assign to this card.</param>
    public void InitializeCard(CardData data)
    {
        cardData = data;
        ResetCard();
    }

    /// <summary>
    /// Handles click events on the card.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        GameEvents.OnCardClicked?.Invoke(this);
    }

    /// <summary>
    /// Handles pointer enter events (hover).
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        outlineComponent.effectColor = Color.yellow;
    }

    /// <summary>
    /// Handles pointer exit events (end hover).
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        outlineComponent.effectColor = Color.black;
    }

    /// <summary>
    /// Handles pointer move events.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerMove(PointerEventData eventData)
    {
        //print($"{cardData.name} Move!");
    }

    /// <summary>
    /// Marks the card as matched and disables it.
    /// </summary>
    public void SetMatched()
    {
        IsMatched = true;
        isInteractable = false;
    }

    /// <summary>
    /// Resets the card to its initial state.
    /// </summary>
    public void ResetCard()
    {
        IsFlipped = false;
        isInteractable = true;
        hasBeenRevealedOnce = false;
        IsFlippedByPlayer = false;
        IsMatched = false;
        imageComponent.enabled = true;
        outlineComponent.enabled = true;

        imageComponent.sprite = cardBack;
        outlineComponent.effectColor = Color.black;
    }

    /// <summary>
    /// Hides the card completely (used when matched).
    /// </summary>
    public void HideCard()
    {
        imageComponent.enabled = false;
        outlineComponent.enabled = false;
    }

    /// <summary>
    /// Shows visual feedback for a successful match.
    /// </summary>
    public void ShowMatchFeedback()
    {
        outlineComponent.effectColor = Color.green;
        SetMatched();
    }

    /// <summary>
    /// Shows visual feedback for a mismatch.
    /// </summary>
    public void ShowMismatchFeedback()
    {
        outlineComponent.effectColor = Color.red;
        IsFlippedByPlayer = false;
    }
}