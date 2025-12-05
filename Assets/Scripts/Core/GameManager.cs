using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Data")]
    [SerializeField] private CardGenerator cardGenerator;

    private List<CardBehaviour> CardsInGame;
    public Action<CardBehaviour> HasPressedOnCard;

    private CardBehaviour currentlyHeldCard;

    private int matches;
    private int turn;
    private int numberOfMatches;

    // Number of Columns and Rows
    // Score and Turns
    // Currently Selected Card and Compare On Click
    // Start is called before the first frame update

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        CardsInGame = cardGenerator.GenerateCards(4,4) as List<CardBehaviour>;
        numberOfMatches = CardsInGame.Count / 2;
    }

    private void OnEnable()
    {
        HasPressedOnCard += OnHasPressedCard;
    }

    private void OnDisable()
    {
        HasPressedOnCard -= OnHasPressedCard;
    }

    private void OnHasPressedCard(CardBehaviour cardPressed)
    {
        if (!currentlyHeldCard)
        {
            currentlyHeldCard = cardPressed;
            return;
        }
        if(currentlyHeldCard == cardPressed)
            return;

        if(currentlyHeldCard.CardData == cardPressed.CardData)
        {
            cardPressed.SetResolved();
            currentlyHeldCard.SetResolved();
            matches++;
            CheckGameOver();
        }
        else
        {
            currentlyHeldCard.FlipCardAnimation();
            cardPressed.FlipCardAnimation();
        }
        turn++;
        currentlyHeldCard = null;
    }

    private void CheckGameOver()
    {
        if (matches >= numberOfMatches)
            print("gg");

    }
}
