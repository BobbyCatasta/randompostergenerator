using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the core game logic including points, turns, saving and loading.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Data")]
    [Tooltip("Card generator for the game scene.")]
    [SerializeField] private CardGenerator cardGenerator;
    [Tooltip("Random suit generator for cards.")]
    [SerializeField] private RandomSuitGenerator randomSuitGenerator;
    [Tooltip("Game text manager for points and turns display.")]
    [SerializeField] private GameTextManager gameTextManager;

    /// <summary>
    /// System for controlling card matching logic.
    /// </summary>
    private MatchSystem matchSystem;

    /// <summary>
    /// Number of columns in the card grid.
    /// </summary>
    private int nColumns = 4;
    /// <summary>
    /// Number of rows in the card grid.
    /// </summary>
    private int nRows = 4;

    /// <summary>
    /// Current player score.
    /// </summary>
    private int points;
    /// <summary>
    /// Number of turns elapsed.
    /// </summary>
    private int turn;

    /// <summary>
    /// Point increment for consecutive matches.
    /// </summary>
    private int pointIncrease = 1;

    /// <summary>
    /// Indicates if X-Ray cheat mode is active.
    /// </summary>
    private bool xRayEnabled;
   

    /// <summary>
    /// Initializes the game manager and dependent systems.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        matchSystem = new MatchSystem();
        randomSuitGenerator.Initialize();
    }
    
    /// <summary>
    /// Starts the game based on boot data (new or loaded).
    /// </summary>
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
    /// Toggles the X-Ray cheat mode to see all cards.
    /// </summary>
    /// <returns></returns>
    /// 
    public void ToggleXRay()
    {
        if (matchSystem.IsAnimating)
            return;
        xRayEnabled = !xRayEnabled;
        CheatXRay(xRayEnabled);
    }

    /// <summary>
    /// Subscribes to game events.
    /// </summary>
    private void OnEnable()
    {
        GameEvents.OnCardClicked += OnHasPressedCard;

        matchSystem.OnMatchFound += OnMatchFound;
        matchSystem.OnMismatch += OnMismatch;
        matchSystem.OnAllPairsCompleted += OnGameCompleted;
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDisable()
    {
        GameEvents.OnCardClicked -= OnHasPressedCard;

        matchSystem.OnMatchFound -= OnMatchFound;
        matchSystem.OnMismatch -= OnMismatch;
        matchSystem.OnAllPairsCompleted -= OnGameCompleted;
    }

    /// <summary>
    /// Handles game completion event.
    /// </summary>
    private void OnGameCompleted()
    {
        AudioManager.Instance.PlaySound(MatchingCardsSound.GameOver);
        GameEvents.GameEnded?.Invoke();
        SaveSystem.DeleteSave();
    }

    /// <summary>
    /// Handles mismatch event between two cards.
    /// </summary>
    private void OnMismatch()
    {
        AdvanceTurn();
        pointIncrease = 1;
    }

    /// <summary>
    /// Handles match found event between two cards.
    /// </summary>
    private void OnMatchFound()
    {
        AdvanceTurn();
        points += pointIncrease;
        gameTextManager.UpdatePointsText(points);
        pointIncrease++;
    }

    /// <summary>
    /// Advances the turn counter.
    /// </summary>
    private void AdvanceTurn()
    {
        turn++;
        gameTextManager.UpdateTurnText(turn);
    }
    
    /// <summary>
    /// Starts a new game with initial values.
    /// </summary>
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
    /// Handles card click event from player.
    /// </summary>
    /// <param name="cardPressed">The card that was clicked.</param>
    private void OnHasPressedCard(I_Card cardPressed)
    {
        matchSystem.HandleCardSelected(cardPressed as CardBehaviour);
        AudioManager.Instance.PlaySound(MatchingCardsSound.CardFlip);
    }

    /// <summary>
    /// Starts a coroutine from the GameManager instance.
    /// </summary>
    /// <param name="routine">The coroutine to start.</param>
    /// <returns>The started coroutine.</returns>
    public Coroutine StartRoutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    /// <summary>
    /// Saves the current game state.
    /// </summary>
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

    /// <summary>
    /// Loads a saved game state.
    /// </summary>
    /// <returns>True if game was successfully loaded, false otherwise.</returns>
    public bool LoadGame()
    {
        // Attempt to load save data from file system
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData == null)
            return false;

        // Restore basic game configuration from save data
        nRows = saveData.nRows;
        nColumns = saveData.nColumns;
        turn = saveData.turn;
        points = saveData.points;
        pointIncrease = saveData.pointIncrease;

        // Update UI with loaded values
        gameTextManager.UpdatePointsText(points);
        gameTextManager.UpdateTurnText(turn);

        // Reconstruct the list of card suits from saved IDs
        List<CardData> suitList = new List<CardData>();

        foreach (var cardState in saveData.cardsState)
        {
            // Look up each card's data using its saved ID
            CardData suit = randomSuitGenerator.GetCardByID(cardState.suitID);
            suitList.Add(suit);
        }

        // Convert list to queue for card generator
        Queue<CardData> queueSuits = new Queue<CardData>(suitList);
        
        // Generate cards with the restored suits
        cardGenerator.GenerateCardsAndSuits(nRows, nColumns, queueSuits);

        // Restore each card's individual state from save data
        int cardsMatched = 0;
        for (int i = 0; i < saveData.cardsState.Count; i++)
        {
            var card = cardGenerator.CardsInGame[i];
            var state = saveData.cardsState[i];

            // Step 7a: Handle matched cards (already found pairs)
            if (state.isMatched)
            {
                card.SetMatched();     // Mark as matched
                card.HideCard();       // Hide from view
                cardsMatched++;        // Count for match system initialization
                continue;              // Skip to next card
            }
            
            // Handle cards that were selected as first card in a turn
            if (state.hasBeenSelected)
            {
                // Show the card face up (it was flipped by player)
                card.ShowFront(true);
                // Restore match system's first card reference
                matchSystem.RestoreFirstSelection(card);
                continue;
            }
            
            // Handle cards that were flipped by player but not selected
            if (state.flippedByPlayer)
            {
                // Show card face up but not as active selection
                card.ShowFront(false);
                continue;
            }
            
            // Default state - show card back (face down)
            card.ShowBack();
        }
        
        // Initialize match system with restored progress
        matchSystem.Initialize(nRows * nColumns, cardsMatched / 2);
        
        return true;
    }


    /// <summary>
    /// Cheat function to enable/disable X-Ray view of cards.
    /// </summary>
    /// <param name="isEnabled">Whether X-Ray is enabled.</param>
    private void CheatXRay(bool isEnabled)
    {
#if UNITY_EDITOR
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
#endif
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void ReturnToMenu()
    {
        SaveGame();
        SceneManager.LoadScene("MenuScene");
    }



    /// <summary>
    /// Saves game when application quits.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}