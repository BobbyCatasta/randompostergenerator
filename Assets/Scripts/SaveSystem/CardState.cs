using System;

/// <summary>
/// Serializable struct representing the state of a single card for saving/loading.
/// </summary>
[Serializable]
public struct CardState
{
    /// <summary>
    /// ID of the card's suit/type.
    /// </summary>
    public string suitID;    
    
    /// <summary>
    /// Whether the card has been matched.
    /// </summary>
    public bool isMatched;
    
    /// <summary>
    /// Whether the card was flipped by player action.
    /// </summary>
    public bool flippedByPlayer;
    
    /// <summary>
    /// Whether the card has been selected (first card in a turn).
    /// </summary>
    public bool hasBeenSelected;
    

    /// <summary>
    /// Constructor for creating a CardState.
    /// </summary>
    /// <param name="suitID">ID of the card's suit.</param>
    /// <param name="flippedByPlayer">Whether flipped by player.</param>
    /// <param name="isMatched">Whether matched.</param>
    /// <param name="hasBeenSelected">Whether selected as first card.</param>
    public CardState(string suitID, bool flippedByPlayer, bool isMatched,bool hasBeenSelected)
    {
        this.suitID = suitID;
        this.flippedByPlayer = flippedByPlayer;
        this.isMatched = isMatched;
        this.hasBeenSelected = hasBeenSelected;
    }
}