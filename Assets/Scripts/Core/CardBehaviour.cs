using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Image displayImage;

    /// <summary>
    /// 
    /// </summary>
    private CardData cardData;

    public void InitializeCard(CardData data)
    {
        // Setting up data on this Card Object
        cardData = data;
        displayImage.sprite = data.CardImage;
        displayImage.preserveAspect = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print($"{cardData.name} Click!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //print($"{cardData.name} Enter!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //print($"{cardData.name} Exit!");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //print($"{cardData.name} Move!");
    }
}
