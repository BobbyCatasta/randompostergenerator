using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Outline))]
public class CardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    /// <summary>
    /// 
    /// </summary>

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Sprite cardBack;

    /// <summary>
    /// 
    /// </summary>
    private Outline outlineComponent;
    
    /// <summary>
    /// 
    /// </summary>
    private Image imageComponent;

    /// <summary>
    /// 
    /// </summary>
    private CardData cardData;


    /// <summary>
    /// 
    /// </summary>
    private bool isInteractable = true;

    /// <summary>
    /// 
    /// </summary>
    public CardData CardData => cardData;


    public bool IsFlipped { get; private set; }
    public bool IsInteractable => isInteractable;

    public bool WasFlippedByPlayer { get; private set; }


    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        outlineComponent = GetComponent<Outline>();
        imageComponent.preserveAspect = true;
    }

    public void ShowFront(bool flippedByPlayer)
    {
        imageComponent.sprite = cardData.CardSprite;
        outlineComponent.effectColor = Color.black;
        IsFlipped = true;
        if (flippedByPlayer)
            WasFlippedByPlayer = true;
    }

    public void ShowBack()
    {
        imageComponent.sprite = cardBack;
        outlineComponent.effectColor = Color.black;
        IsFlipped = false;
    }

    public void DisableCard()
    {
        isInteractable = false;
    }

    public void EnableCard()
    {
        isInteractable = true;
    }

    public void InitializeCard(CardData data)
    {
        cardData = data;
        ResetCard();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        GameManager.Instance.HasPressedOnCard?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        outlineComponent.effectColor = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        outlineComponent.effectColor = Color.black;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //print($"{cardData.name} Move!");
    }

    public void ResetCard()
    {
        IsFlipped = false;
        isInteractable = true;
        WasFlippedByPlayer = false;

        imageComponent.enabled = true;
        outlineComponent.enabled = true;

        imageComponent.sprite = cardBack;
        outlineComponent.effectColor = Color.black;
    }

    public void HideCard()
    {
        imageComponent.enabled = false;
        outlineComponent.enabled = false;
    }

    public void ShowMatchFeedback()
    {
        outlineComponent.effectColor = Color.green;
    }

    public void ShowMismatchFeedback()
    {
        outlineComponent.effectColor = Color.red;
        WasFlippedByPlayer = false;
    }
}
