using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Data")]
    [SerializeField] private CardGenerator cardGenerator;
    [SerializeField] private RandomSuitGenerator randomSuitGenerator;
    [SerializeField] private GameTextManager gameTextManager;

    private MatchSystem matchSystem;

    private int nColumns = 4;
    private int nRows = 4;

    private int points;
    private int turn;

    private int pointIncrease = 1;

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
        GameEvents.OnCardClicked += OnHasPressedCard;

        matchSystem.OnMatchFound += OnMatchFound;
        matchSystem.OnMismatch += OnMismatch;
        matchSystem.OnAllPairsCompleted += OnGameCompleted;
    }


    private void OnDisable()
    {
        GameEvents.OnCardClicked -= OnHasPressedCard;

        matchSystem.OnMatchFound -= OnMatchFound;
        matchSystem.OnMismatch -= OnMismatch;
        matchSystem.OnAllPairsCompleted -= OnGameCompleted;
    }

    private void OnGameCompleted()
    {
        AudioManager.Instance.PlaySound(MatchingCardsSound.GameOver);
        GameEvents.GameEnded?.Invoke();
        SaveSystem.DeleteSave();
    }



    private void OnMismatch()
    {
        AdvanceTurn();
        pointIncrease = 1;
    }

    private void OnMatchFound()
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
        cardGenerator.GenerateCardsAndSuits(nRows, nColumns,queueCards);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardPressed"></param>
    private void OnHasPressedCard(I_Card cardPressed)
    {
        matchSystem.HandleCardSelected(cardPressed as CardBehaviour);
        AudioManager.Instance.PlaySound(MatchingCardsSound.CardFlip);
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

        List<CardData> suitList = new List<CardData>();

        foreach (var cardState in saveData.cardsState)
        {
            CardData suit = randomSuitGenerator.GetCardByID(cardState.suitID);
            suitList.Add(suit);
        }

        Queue<CardData> queueSuits = new Queue<CardData>(suitList);
        cardGenerator.GenerateCardsAndSuits(nRows, nColumns, queueSuits);

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
        SaveGame();
        SceneManager.LoadScene("MenuScene");
    }

#endif

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
