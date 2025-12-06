using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ScriptableObject that generates and manages random card suits for the game.
/// </summary>
[CreateAssetMenu(fileName = "Card Suits",menuName = "Card Game/SuitGenerator")]
public class RandomSuitGenerator : ScriptableObject
{
    [Tooltip("List of available card data (suits).")]
    [SerializeField] private List<CardData> cardSuits;
    
    /// <summary>
    /// Dictionary mapping card IDs to their CardData.
    /// </summary>
    public Dictionary<string, CardData> IDSuitToSuit { get; private set; }

    /// <summary>
    /// Initializes the dictionary of card suits.
    /// </summary>
    public void Initialize()
    {
        IDSuitToSuit = new Dictionary<string, CardData>();

        foreach (var seed in cardSuits)
        {
            if (!IDSuitToSuit.ContainsKey(seed.ID))
                IDSuitToSuit.Add(seed.ID, seed);
        }
    }

    /// <summary>
    /// Validates and ensures unique card suits in the editor.
    /// </summary>
    private void OnValidate()
    {
        // Check Unique Suits
        if (cardSuits == null) return;

        for (int i = cardSuits.Count - 1; i >= 0; i--)
        {
            var item = cardSuits[i];

            if (cardSuits.IndexOf(item) != i)
                cardSuits.RemoveAt(i);
        }
    }

    /// <summary>
    /// Gets CardData by its ID.
    /// </summary>
    /// <param name="id">The ID of the card to retrieve.</param>
    /// <returns>The CardData with the specified ID.</returns>
    public CardData GetCardByID(string id)
    {
        return IDSuitToSuit[id];
    }

    /// <summary>
    /// Shuffles a list of CardData using Fisher-Yates algorithm.
    /// </summary>
    /// <param name="list">The list of CardData to shuffle.</param>
    private void ShuffleSeeds(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// Generates randomized pairs of card suits for the game.
    /// </summary>
    /// <param name="numberOfCards">Total number of cards needed.</param>
    /// <returns>A queue of randomized CardData for card generation.</returns>
    public Queue<CardData> GenerateRandomizedSuits(in int numberOfCards)
    {
        // Validate that we have enough unique card types
        if (numberOfCards / 2 > cardSuits.Count)
        {
            Debug.LogError("Not enough suits to generate pairs");
            return null;
        }
        
        // Prepare lists for card selection
        List<CardData> selectedCardSeeds = new List<CardData>();     // Final list of all cards (with duplicates for pairs)
        List<CardData> currentPossibleSeeds = new List<CardData>(cardSuits); // Available unique card types

        // Generate pairs of cards
        for (int i = 0; i < numberOfCards / 2; i++)
        {
            // Randomly select a card type from available options
            int randomIndex = Random.Range(0, currentPossibleSeeds.Count);

            // Add two copies of the selected card type (creating a pair)
            selectedCardSeeds.Add(currentPossibleSeeds[randomIndex]);
            selectedCardSeeds.Add(currentPossibleSeeds[randomIndex]);
            
            // Remove the selected type from available options
            currentPossibleSeeds.RemoveAt(randomIndex);
        }
        
        // Shuffle the final list to randomize card positions
        ShuffleSeeds(selectedCardSeeds);
        
        // Convert to queue for sequential distribution during card generation
        return new Queue<CardData>(selectedCardSeeds);
    }
}