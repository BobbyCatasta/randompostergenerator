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
    public CardData CardData => cardData;

    public bool HasBeenResolved;

    

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
        if (HasBeenResolved)
            return;
        StartCoroutine(FlipCardCoroutine());
        GameManager.Instance.HasPressedOnCard?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasBeenResolved || hasBeenClicked)
            return;
        outlineComponent.effectColor = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HasBeenResolved || hasBeenClicked)
            return;
        outlineComponent.effectColor = Color.black;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //print($"{cardData.name} Move!");
    }

    public void FlipCardAnimation()
    {
        StartCoroutine(FlipCardCoroutine());
    }

    private IEnumerator FlipCardCoroutine()
    {
        if (hasBeenClicked)
        {
            imageComponent.sprite = cardBack;
            outlineComponent.effectColor = Color.black;
            hasBeenClicked = false;
        }
        else
        {
            imageComponent.sprite = cardData.CardSprite;
            hasBeenClicked = true;
        }
        yield return null;
    }

    public void SetResolved()
    {
        HasBeenResolved = true;
        outlineComponent.effectColor = Color.green;
    }
}
