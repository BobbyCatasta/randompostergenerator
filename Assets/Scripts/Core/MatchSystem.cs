using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles card matching logic and game progression.
/// </summary>
public class MatchSystem
{
    /// <summary>
    /// First selected card in the current turn.
    /// </summary>
    private I_Card firstCard;
    
    /// <summary>
    /// Second selected card in the current turn.
    /// </summary>
    private I_Card secondCard;

    /// <summary>
    /// Delay before flipping back mismatched cards.
    /// </summary>
    private float flipBackDelay = .4f;

    /// <summary>
    /// Event triggered when a match is found.
    /// </summary>
    public event Action OnMatchFound;
    
    /// <summary>
    /// Event triggered when cards don't match.
    /// </summary>
    public event Action OnMismatch;
    
    /// <summary>
    /// Event triggered when all pairs are completed.
    /// </summary>
    public event Action OnAllPairsCompleted;

    /// <summary>
    /// Total number of pairs in the game.
    /// </summary>
    private int totalPairs;
    
    /// <summary>
    /// Number of matched pairs found so far.
    /// </summary>
    private int matchedPairs;

    /// <summary>
    /// Indicates if an animation is currently playing.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    /// Public accessor for the first selected card.
    /// </summary>
    public I_Card FirstCard => firstCard;

    /// <summary>
    /// Initializes the match system with game parameters.
    /// </summary>
    /// <param name="numberOfCards">Total number of cards in the game.</param>
    /// <param name="matchedPairs">Number of already matched pairs (for loaded games).</param>
    public void Initialize(in int numberOfCards, int matchedPairs = 0)
    {
        this.matchedPairs = matchedPairs;
        totalPairs = numberOfCards / 2;

        secondCard = null;
        IsAnimating = false;
    }

    /// <summary>
    /// Handles card selection by the player.
    /// </summary>
    /// <param name="card">The card that was selected.</param>
    public void HandleCardSelected(I_Card card)
    {
        if (card == firstCard)
            return;

        if (firstCard == null)
        {
            firstCard = card;
            card.ShowFront(true);
            return;
        }

        secondCard = card;
        secondCard.ShowFront(true);

        CheckMatch();
    }

    /// <summary>
    /// Checks if the two selected cards match.
    /// </summary>
    private void CheckMatch()
    {
        firstCard.DisableCard();
        secondCard.DisableCard();

        if (firstCard.ID == secondCard.ID)
        {
            matchedPairs++;
            OnMatchFound?.Invoke();
            AudioManager.Instance.PlaySound(MatchingCardsSound.Match);
            GameManager.Instance.StartRoutine(HideCardCoroutine());

            if (matchedPairs >= totalPairs)
                OnAllPairsCompleted?.Invoke();
        }
        else
        {
            AudioManager.Instance.PlaySound(MatchingCardsSound.Mismatch);
            OnMismatch?.Invoke();
            GameManager.Instance.StartRoutine(FlipBackCoroutine());
        }
    }

    /// <summary>
    /// Restores the first card selection state (used when loading saved games).
    /// </summary>
    /// <param name="card">The card to set as first selected.</param>
    public void RestoreFirstSelection(CardBehaviour card)
    {
        firstCard = card;
    }

    /// <summary>
    /// Coroutine for flipping back mismatched cards.
    /// </summary>
    private IEnumerator FlipBackCoroutine()
    {
        IsAnimating = true;

        var pendingFirstCard = firstCard;
        var pendingSecondCard = secondCard;

        pendingFirstCard.ShowMismatchFeedback();
        pendingSecondCard.ShowMismatchFeedback();

        ClearSelection();
        yield return new WaitForSecondsRealtime(flipBackDelay);

        pendingFirstCard.ShowBack();
        pendingSecondCard.ShowBack();

        pendingFirstCard.EnableCard();
        pendingSecondCard.EnableCard();

        IsAnimating = false;
    }

    /// <summary>
    /// Coroutine for hiding matched cards.
    /// </summary>
    private IEnumerator HideCardCoroutine()
    {
        IsAnimating = true;

        var pendingFirstCard = firstCard;
        var pendingSecondCard = secondCard;

        pendingFirstCard.ShowMatchFeedback();
        pendingSecondCard.ShowMatchFeedback();

        ClearSelection();

        yield return new WaitForSecondsRealtime(flipBackDelay);

        pendingFirstCard.HideCard();
        pendingSecondCard.HideCard();

        IsAnimating = false;

    }

    /// <summary>
    /// Clears the current card selection.
    /// </summary>
    private void ClearSelection()
    {
        firstCard = null;
        secondCard = null;
    }
}