using UnityEngine;

/// <summary>
/// Interface defining the contract for card game objects.
/// </summary>
public interface I_Card
{
    /// <summary>
    /// Unique identifier for the card.
    /// </summary>
    public string ID { get; }
    
    /// <summary>
    /// Shows the front (face) of the card.
    /// </summary>
    /// <param name="flippedByPlayer">Whether the card was flipped by player action.</param>
    public void ShowFront(bool flippedByPlayer);
    
    /// <summary>
    /// Shows the back (hidden) of the card.
    /// </summary>
    public void ShowBack();
    
    /// <summary>
    /// Disables card interaction.
    /// </summary>
    public void DisableCard();
    
    /// <summary>
    /// Hides the card completely (when matched).
    /// </summary>
    public void HideCard();
    
    /// <summary>
    /// Enables card interaction.
    /// </summary>
    public void EnableCard();
    
    /// <summary>
    /// Marks the card as matched.
    /// </summary>
    public void SetMatched();
    
    /// <summary>
    /// Resets the card to its initial state.
    /// </summary>
    public void ResetCard();
    
    /// <summary>
    /// Shows visual feedback for a successful match.
    /// </summary>
    public void ShowMatchFeedback();
    
    /// <summary>
    /// Shows visual feedback for a mismatch.
    /// </summary>
    public void ShowMismatchFeedback();
}