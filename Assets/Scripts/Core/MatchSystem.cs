using System;
using System.Collections;
using UnityEngine;

public class MatchSystem
{
    private I_Card firstCard;
    private I_Card secondCard;

    private float flipBackDelay = .4f;

    public event Action OnMatchFound;
    public event Action OnMismatch;
    public event Action OnAllPairsCompleted;

    private int totalPairs;
    private int matchedPairs;

    public bool IsAnimating { get; private set; }

    public I_Card FirstCard => firstCard;

    public void Initialize(in int numberOfCards, int matchedPairs = 0)
    {
        this.matchedPairs = matchedPairs;
        totalPairs = numberOfCards / 2;

        secondCard = null;
        IsAnimating = false;
    }

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

    public void RestoreFirstSelection(CardBehaviour card)
    {
        firstCard = card;
    }

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

    private void ClearSelection()
    {
        firstCard = null;
        secondCard = null;
    }
}
