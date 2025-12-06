using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        randomSuitGenerator.Initialize();
    }
    private void Start()
    {
        nColumns = GameBoot.Columns;
        nRows = GameBoot.Rows;
        cardGenerator.GenerateCards(nRows, nColumns);
        if (GameBoot.IsNewGame)
            StartGame();
        else
            LoadGame();
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
        SaveSystem.DeleteSave();
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
        SaveGame();
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


    public void SaveGame()
    {
        SaveData data = new SaveData();

        data.nColumns = nColumns;
        data.nRows = nRows;
        data.cardsState = new List<CardState>();

        foreach (var card in cardGenerator.CardsInGame)
        {
            var cardState = new CardState(card.CardData.ID, card.IsFlippedByPlayer, card.IsMatched,card == matchSystem.FirstCard);
            data.cardsState.Add(cardState);
        }
        data.turn = turn;
        data.pointIncrease = pointIncrease;
        data.points = points;

        SaveSystem.SaveGame(data);
    }

    public bool LoadGame()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
            return false;

        nRows = saveData.nRows;
        nColumns = saveData.nColumns;
        turn = saveData.turn;
        points = saveData.points;
        pointIncrease = saveData.pointIncrease;

        gameTextManager.UpdatePointsText(points);
        gameTextManager.UpdateTurnText(turn);

        Queue<CardData> queue = new Queue<CardData>();

        foreach (var cardState in saveData.cardsState)
        {
            CardData seed = randomSuitGenerator.GetCardByID(cardState.suitID);
            queue.Enqueue(seed);
        }
        cardGenerator.SetSuits(queue);

        int cardsMatched = 0;
        for (int i = 0; i < saveData.cardsState.Count; i++)
        {
            var card = cardGenerator.CardsInGame[i];
            var state = saveData.cardsState[i];

            if (state.isMatched)
            {
                card.SetMatched();
                card.HideCard();
                cardsMatched++;
                continue;
            }
            if (state.hasBeenSelected)
            {
                card.ShowFront(true);
                matchSystem.RestoreFirstSelection(card);
                continue;
            }
            if (state.flippedByPlayer)
            {
                card.ShowFront(false);
                continue;
            }
            card.ShowBack();

        }
        matchSystem.Initialize(nRows * nColumns, cardsMatched / 2);
        return true;
    }

#if UNITY_EDITOR
    private void CheatXRay(bool isEnabled)
    {
        foreach (var card in cardGenerator.CardsInGame)
        {
            if (!card.IsInteractable)
                continue;

            if (isEnabled)
            {
                if (!card.IsMatched)
                    card.ShowFront(false);
            }
            else
            {
                if (!card.IsFlippedByPlayer && !card.IsMatched)
                    card.ShowBack();
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

#endif

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
