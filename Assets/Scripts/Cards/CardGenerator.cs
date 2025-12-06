using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generates and manages the grid of cards in the game scene.
/// </summary>
public class CardGenerator : MonoBehaviour
{
    [Header("Grid Layout Settings")]
    [Min(10)][SerializeField] private float MIN_CELL_SIZE = 30;
    [SerializeField] private float MAX_CELL_SIZE = 200;
    [Min(10)][SerializeField] private float MIN_SPACING = 30;
    [SerializeField] private float MAX_SPACING = 200;

    [Header("Card Generation")]
    [Tooltip("Prefab for card instances.")]
    [SerializeField] private CardBehaviour cardPrefab;
    
    [Tooltip("Grid layout component for arranging cards.")]
    [SerializeField] private GridLayoutGroup gridLayout;

    /// <summary>
    /// List of all card instances currently in the game.
    /// </summary>
    private List<CardBehaviour> cardsInGame = new List<CardBehaviour>();

    /// <summary>
    /// Read-only accessor for cards in game.
    /// </summary>
    public IReadOnlyList<CardBehaviour> CardsInGame => cardsInGame;

    /// <summary>
    /// Validates serialized fields in the editor.
    /// </summary>
    private void OnValidate()
    {
        // Set max cell size to min
        if (MAX_CELL_SIZE < MIN_CELL_SIZE)
            MAX_CELL_SIZE = MIN_CELL_SIZE;
        if (MAX_SPACING < MIN_SPACING)
            MAX_SPACING = MIN_SPACING;
    }

    /// <summary>
    /// Assigns suits to already generated cards.
    /// </summary>
    /// <param name="queueSuits">Queue of CardData to assign to cards.</param>
    public void SetSuits(Queue<CardData> queueSuits)
    {
        if (queueSuits.Count != cardsInGame.Count)
        {
            Debug.LogError($"Suit mismatch! Cards: {cardsInGame.Count}, Queue: {queueSuits.Count}");
            return;
        }
        foreach (CardBehaviour card in cardsInGame)
        {
            card.InitializeCard(queueSuits.Dequeue());
        }
    }

    /// <summary>
    /// Generates card game objects based on grid dimensions.
    /// </summary>
    /// <param name="nOfRows">Number of rows in the grid.</param>
    /// <param name="nOfColumns">Number of columns in the grid.</param>
    private void GenerateCards(in int nOfRows, in int nOfColumns)
    {
        cardsInGame.Clear();
        int numberOfCards = nOfColumns * nOfRows;
        SetSpacingAndColumns(nOfColumns, nOfRows);

        for (int i = 0; i < numberOfCards; i++)
        {
            CardBehaviour card = Instantiate(cardPrefab, gridLayout.transform);
            cardsInGame.Add(card);
        }
    }

    /// <summary>
    /// Generates cards and assigns suits in one operation.
    /// </summary>
    /// <param name="nOfRows">Number of rows in the grid.</param>
    /// <param name="nOfColumns">Number of columns in the grid.</param>
    /// <param name="queueSuits">Queue of CardData for card suits.</param>
    public void GenerateCardsAndSuits(in int nOfRows, in int nOfColumns, in Queue<CardData> queueSuits)
    {
        if(cardsInGame.Count <= 0)
            GenerateCards(nOfRows, nOfColumns);
        SetSuits(queueSuits);
    }

    /// <summary>
    /// Calculates and sets optimal grid layout parameters.
    /// </summary>
    /// <param name="nOfColumns">Number of columns in the grid.</param>
    /// <param name="nOfRows">Number of rows in the grid.</param>
    private void SetSpacingAndColumns(in int nOfColumns, in int nOfRows)
    { 
        RectTransform rect = gridLayout.GetComponent<RectTransform>();

        float width = rect.rect.width;
        float height = rect.rect.height;

        // Calculate ideal cell size based on available space
        float idealCellWidth = width / nOfColumns;
        // Divide available height by number of rows for height per cell
        float idealCellHeight = height / nOfRows;

        // Clamp cell sizes to stay within defined min/max ranges
        float clampedWidth = Mathf.Clamp(idealCellWidth, MIN_CELL_SIZE, MAX_CELL_SIZE);
        float clampedHeight = Mathf.Clamp(idealCellHeight, MIN_CELL_SIZE, MAX_CELL_SIZE);

        // Use the smaller dimension to maintain square cells
        float clampedUsedMeasure = Mathf.Min(clampedWidth, clampedHeight);

        // Apply the calculated square cell size
        Vector2 finalCellSize = new Vector2(clampedUsedMeasure, clampedUsedMeasure);
        gridLayout.cellSize = finalCellSize;

        // Step 5: Calculate how much space the cells will actually occupy
        float usedWidth = nOfColumns * finalCellSize.x;
        float usedHeight = nOfRows * finalCellSize.y;
        
        // Step 6: Calculate remaining free space after placing cells
        // If cells are larger than available space, free space becomes 0
        float freeWidth = Mathf.Max(0, width - usedWidth);
        float freeHeight = Mathf.Max(0, height - usedHeight);

        // Calculate spacing between cells
        float spacingX = nOfColumns > 1 ? freeWidth / (nOfColumns - 1) : 0;
        float spacingY = nOfRows > 1 ? freeHeight / (nOfRows - 1) : 0;

        // Clamp spacing to stay within defined min/max ranges
        spacingX = Mathf.Clamp(spacingX, MIN_SPACING, MAX_SPACING);
        spacingY = Mathf.Clamp(spacingY, MIN_SPACING, MAX_SPACING);

        // Apply the calculated spacing
        gridLayout.spacing = new Vector2(spacingX, spacingY);

        // Configure grid layout constraints
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = nOfColumns;
    }
}