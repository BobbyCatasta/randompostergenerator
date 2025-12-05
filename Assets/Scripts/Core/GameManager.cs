using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Data")]
    [SerializeField] private CardGenerator cardGenerator;
    [SerializeField] private MatchTextManager matchTextManager;

    public Action<CardBehaviour> HasPressedOnCard;
    public Action GameStart;
    public Action GameEnded;
    private CardBehaviour currentlyHeldCard;

    private List<CardBehaviour> cardsInGame; // -> SERVE DAVVERO?
    private int points;
    private int turn;
    private int matchesCounter;
    private int numberOfMatches;

    // COMBO
    private int pointIncrease = 1;

    // Number of Columns and Rows
    // Score and Turns
    // Currently Selected Card and Compare On Click
    // Start is called before the first frame update

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        StartGame();
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheatXRay(bool isEnabling)
    {
        foreach(CardBehaviour element in cardsInGame)
        {
            if (isEnabling)
            {
                if (!element.IsFlipped)
                    element.FlipCardAnimation();
            }
            else
            {
                if (element == currentlyHeldCard)
                    continue;
                element.FlipCardAnimation();
            }
        }
    }

    private void OnEnable()
    {
        HasPressedOnCard += OnHasPressedCard;
    }

    private void OnDisable()
    {
        HasPressedOnCard -= OnHasPressedCard;
    }

    public void StartGame()
    {
        GameStart?.Invoke();

        points = 0;
        pointIncrease = 1;
        turn = 0;
        matchesCounter = 0;

        matchTextManager.UpdatePointsText(points);
        matchTextManager.UpdateTurnText(turn);

        cardsInGame = cardGenerator.GenerateCards(4, 4) as List<CardBehaviour>;
        numberOfMatches = cardsInGame.Count / 2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardPressed"></param>
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
            cardsInGame.Remove(currentlyHeldCard);
            cardsInGame.Remove(cardPressed);

            currentlyHeldCard.GuessedCard();
            cardPressed.GuessedCard();

            points += pointIncrease;
            pointIncrease++;
            matchesCounter++;

            matchTextManager.UpdatePointsText(points);
            CheckGameOver();
        }
        else
        {
            pointIncrease = 1;
            currentlyHeldCard.DelayedFlipCardAnimation();
            cardPressed.DelayedFlipCardAnimation();
        }
        turn++;
        matchTextManager.UpdateTurnText(turn);
        currentlyHeldCard = null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckGameOver()
    {
        if (matchesCounter >= numberOfMatches)
            GameEnded?.Invoke();

    }
}
