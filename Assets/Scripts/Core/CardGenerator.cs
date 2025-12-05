using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    [Range(1,10)][SerializeField] private int nOfColumns;
    [Range(1,10)][SerializeField] private int nOfRows;

    [SerializeField] private float MIN_CELL_SIZE = 50;
    [SerializeField] private float MAX_CELL_SIZE = 200;
    [SerializeField] private float MIN_SPACING = 20;
    [SerializeField] private float MAX_SPACING = 200;

    [SerializeField] private CardData templateDataCard;

    [SerializeField] private CardBehaviour cardPrefab;
    [SerializeField] private GridLayoutGroup gridLayout;



    private void Start()
    {
        GenerateCards(); //DEBUG PURPOSE ONLY
    }
    public void GenerateCards()
    {
        SetSpacingAndColumns();

        int numberOfCards = nOfColumns * nOfRows;

        for(int i = 0;i< numberOfCards; i ++)
        {
            CardBehaviour card = Instantiate(cardPrefab, gridLayout.transform);
            card.InitializeCard(templateDataCard);
        }
    }

    private void SetSpacingAndColumns()
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

        Vector2 finalCellSize = new Vector2(clampedWidth, clampedHeight);
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
