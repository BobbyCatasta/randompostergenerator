using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    [Min(10)][SerializeField] private float MIN_CELL_SIZE = 30;
    [SerializeField] private float MAX_CELL_SIZE = 200;
    [Min(10)][SerializeField] private float MIN_SPACING = 30;
    [SerializeField] private float MAX_SPACING = 200;

    [SerializeField] private CardBehaviour cardPrefab;
    [SerializeField] private GridLayoutGroup gridLayout;

    private List<CardBehaviour> cardsInGame = new List<CardBehaviour>();

    public IReadOnlyList<CardBehaviour> CardsInGame => cardsInGame;

    /// <summary>
    /// 
    /// </summary>
    private void OnValidate()
    {
        // Set max cell size to min
        if (MAX_CELL_SIZE < MIN_CELL_SIZE)
            MAX_CELL_SIZE = MIN_CELL_SIZE;
        if (MAX_SPACING < MIN_SPACING)
            MAX_SPACING = MIN_SPACING;
    }

    public void SetSuits(Queue<CardData> queueSuits)
    {
        foreach(CardBehaviour card in cardsInGame)
        {
            card.InitializeCard(queueSuits.Dequeue());
        }
    }

    public void GenerateCards(in int nOfRows, in int nOfColumns)
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
    /// 
    /// </summary>
    /// <param name="nOfColumns"></param>
    /// <param name="nOfRows"></param>
    private void SetSpacingAndColumns(in int nOfColumns, in int nOfRows)
    { 
        // Calculating the space for all cards
        RectTransform rect = gridLayout.GetComponent<RectTransform>();

        float width = rect.rect.width;
        float height = rect.rect.height;

        float idealCellWidth = width / nOfColumns;
        float idealCellHeight = height / nOfRows;

        // Clamping per mantenere cell size entro un range
        float clampedWidth = Mathf.Clamp(idealCellWidth, MIN_CELL_SIZE, MAX_CELL_SIZE);
        float clampedHeight = Mathf.Clamp(idealCellHeight, MIN_CELL_SIZE, MAX_CELL_SIZE);

        float clampedUsedMeasure = Mathf.Min(clampedWidth, clampedHeight);

        Vector2 finalCellSize = new Vector2(clampedUsedMeasure, clampedUsedMeasure);
        gridLayout.cellSize = finalCellSize;

        float usedWidth = nOfColumns * finalCellSize.x;
        float usedHeight = nOfRows * finalCellSize.y;
      

        float freeWidth = Mathf.Max(0, width - usedWidth);
        float freeHeight = Mathf.Max(0, height - usedHeight);


        float spacingX = nOfColumns > 1 ? freeWidth / (nOfColumns - 1) : 0;
        float spacingY = nOfRows > 1 ? freeHeight / (nOfRows - 1) : 0;

        spacingX = Mathf.Clamp(spacingX, MIN_SPACING, MAX_SPACING);
        spacingY = Mathf.Clamp(spacingY, MIN_SPACING, MAX_SPACING);

        gridLayout.spacing = new Vector2(spacingX, spacingY);

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = nOfColumns;
    }

}
