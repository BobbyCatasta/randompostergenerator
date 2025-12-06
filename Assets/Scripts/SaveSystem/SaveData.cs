using System;
using System.Collections.Generic;

/// <summary>
/// Serializable class representing the saved game state.
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// Number of rows in the saved game.
    /// </summary>
    public int nRows;
    
    /// <summary>
    /// Number of columns in the saved game.
    /// </summary>
    public int nColumns;

    /// <summary>
    /// List of card states for each card in the game.
    /// </summary>
    public List<CardState> cardsState;

    /// <summary>
    /// Current turn count.
    /// </summary>
    public int turn;
    
    /// <summary>
    /// Current points.
    /// </summary>
    public int points;
    
    /// <summary>
    /// Current point increase multiplier.
    /// </summary>
    public int pointIncrease;
}