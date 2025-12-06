using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages UI text updates for game statistics.
/// </summary>
public class GameTextManager : MonoBehaviour
{
    [Tooltip("Text component for displaying points.")]
    [SerializeField] private TMP_Text pointsText;
    
    [Tooltip("Text component for displaying turn count.")]
    [SerializeField] private TMP_Text turnText;

    /// <summary>
    /// Updates the points display text.
    /// </summary>
    /// <param name="match">The current points value to display.</param>
    public void UpdatePointsText(in int match)
    {
        pointsText.text = "Points : " + match;
    }

    /// <summary>
    /// Updates the turn count display text.
    /// </summary>
    /// <param name="turn">The current turn count to display.</param>
    public void UpdateTurnText(in int turn)
    {
        turnText.text = "Turn : " + turn;
    }
}