using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Data")]
    [SerializeField] private CardGenerator cardGenerator;
    [SerializeField] private RandomSuitGenerator randomSuitGenerator;
    [SerializeField] private GameTextManager gameTextManager;

    private MatchSystem matchSystem;

    public Action GameEnded;

    private int nColumns = 4;
    private int nRows = 4;

    private int points;
    private int turn;

    private int pointIncrease = 1;

    public Action<CardBehaviour> HasPressedOnCard;


    private bool xRayEnabled;
   

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// 
    protected override void Awake()
    {
        base.Awake();
        matchSystem = new MatchSystem();
    }
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        cardGenerator.GenerateCards(nColumns, nRows);
        StartGame();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ToggleXRay()
    {
        if (matchSystem.IsAnimating)
            return;
        xRayEnabled = !xRayEnabled;
        CheatXRay(xRayEnabled);
    }

    private void OnEnable()
    {
        HasPressedOnCard += OnHasPressedCard;

        matchSystem.OnMatchFound += OnMatchFound;
        matchSystem.OnMismatch += OnMismatch;
        matchSystem.OnAllPairsCompleted += OnGameCompleted;
    }

    private void OnGameCompleted()
    {
        GameEnded?.Invoke();
    }

    private void OnMismatch(CardBehaviour firstCard, CardBehaviour secondCard)
    {
        AdvanceTurn();
        pointIncrease = 1;
    }

    private void OnMatchFound(CardBehaviour firstCard, CardBehaviour secondCard)
    {
        AdvanceTurn();
        points += pointIncrease;
        gameTextManager.UpdatePointsText(points);
        pointIncrease++;
    }

    private void AdvanceTurn()
    {
        turn++;
        gameTextManager.UpdateTurnText(turn);
    }

    private void OnDisable()
    {
        HasPressedOnCard -= OnHasPressedCard;

        matchSystem.OnMatchFound -= OnMatchFound;
        matchSystem.OnMismatch -= OnMismatch;
        matchSystem.OnAllPairsCompleted -= OnGameCompleted;
    }

    public void StartGame()
    {
        points = 0;
        pointIncrease = 1;
        turn = 0;

        gameTextManager.UpdatePointsText(points);
        gameTextManager.UpdateTurnText(turn);

        int numberOfCards = nColumns * nRows;

        matchSystem.Initialize(numberOfCards);
        Queue<CardData> queueCards = randomSuitGenerator.GenerateRandomizedSuits(numberOfCards);
        cardGenerator.SetSuits(queueCards);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardPressed"></param>
    private void OnHasPressedCard(CardBehaviour cardPressed)
    {
        matchSystem.HandleCardSelected(cardPressed);
    }

    public Coroutine StartRoutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }


    private void CheatXRay(bool isEnabled)
    {
        foreach (var card in cardGenerator.CardsInGame)
        {
            if (!card.IsInteractable)
                continue;

            if (isEnabled)
            {
                if (!card.IsFlipped)
                    card.ShowFront(false);
            }
            else
            {
                if (!card.WasFlippedByPlayer)
                    card.ShowBack();
            }
        } 
    }

}
