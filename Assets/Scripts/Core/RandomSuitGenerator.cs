using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Suits",menuName = "Card Game/SuitGenerator")]
public class RandomSuitGenerator : ScriptableObject
{
    [SerializeField] private List<CardData> cardSeeds;


    private void OnValidate()
    {
        // Check Unique Seeds
        if (cardSeeds == null) return;

        for (int i = cardSeeds.Count - 1; i >= 0; i--)
        {
            var item = cardSeeds[i];

            if (cardSeeds.IndexOf(item) != i)
                cardSeeds.RemoveAt(i);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    private void ShuffleSeeds(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Queue<CardData> GenerateRandomizedSuits(in int numberOfCards)
    {
        if (numberOfCards / 2 > cardSeeds.Count)
        {
            Debug.LogError("Not enough suits to generate pairs");
            return null;
        }
        List<CardData> selectedCardSeeds = new List<CardData>();
        List<CardData> currentPossibleSeeds = new List<CardData>(cardSeeds);

        for (int i = 0; i < numberOfCards / 2; i++)
        {
            int randomIndex = Random.Range(0, currentPossibleSeeds.Count);

            selectedCardSeeds.Add(currentPossibleSeeds[randomIndex]);
            selectedCardSeeds.Add(currentPossibleSeeds[randomIndex]);
            currentPossibleSeeds.RemoveAt(randomIndex);
        }
        ShuffleSeeds(selectedCardSeeds);
        return new Queue<CardData>(selectedCardSeeds);
    }
}
