using System;

/// <summary>
/// Static class containing global game events.
/// </summary>
public static class GameEvents
{
    /// <summary>
    /// Event triggered when a card is clicked.
    /// </summary>
    public static Action<I_Card> OnCardClicked;
    
    /// <summary>
    /// Event triggered when the game ends.
    /// </summary>
    public static Action GameEnded;
}