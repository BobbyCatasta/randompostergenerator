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
    [SerializeField] private Image imageComponent;

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
    private CardData cardData;

    /// <summary>
    /// 
    /// </summary>
    private bool hasBeenClicked;

    /// <summary>
    /// 
    /// </summary>
    private bool isInteractable = true;

    /// <summary>
    /// 
    /// </summary>
    public CardData CardData => cardData;

    public bool IsFlipped;





    private void Start()
    {
        outlineComponent = GetComponent<Outline>();
        imageComponent.sprite = cardBack;
        imageComponent.preserveAspect = true;
    }

    public void InitializeCard(CardData data)
    {
        // Setting up data on this Card Object
        cardData = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInteractable)
            return;
        FlipCardAnimation();
        GameManager.Instance.HasPressedOnCard?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractable || hasBeenClicked)
            return;
        outlineComponent.effectColor = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractable || hasBeenClicked)
            return;
        outlineComponent.effectColor = Color.black;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //print($"{cardData.name} Move!");
    }

    public void DelayedFlipCardAnimation()
    {
        isInteractable = false;
        outlineComponent.effectColor = Color.red;
        StartCoroutine(DelayedFlipCoroutine());
        //StartCoroutine(FlipCardCoroutine());
    }

    // TO CHANGE 
    public void FlipCardAnimation()
    {
        if (hasBeenClicked)
        {
            IsFlipped = false;
            imageComponent.sprite = cardBack;
            outlineComponent.effectColor = Color.black;
            hasBeenClicked = false;
        }
        else
        {
            IsFlipped = true;
            imageComponent.sprite = cardData.CardSprite;
            hasBeenClicked = true;
        }
    }

    public void SetResolved()
    {
        isInteractable = true;
        outlineComponent.effectColor = Color.green;
    }

    private IEnumerator DelayedFlipCoroutine()
    {
        yield return new WaitForSecondsRealtime(.5f);
        FlipCardAnimation();
        isInteractable = true;
    }

    public void GuessedCard()
    {
        imageComponent.enabled = false;
        outlineComponent.enabled = false;
        Destroy(this);
    }

    public void SkipDelayed() // -> TO USE?
    {
        StopCoroutine(DelayedFlipCoroutine());
        FlipCardAnimation();
        isInteractable = true;
    }

}
