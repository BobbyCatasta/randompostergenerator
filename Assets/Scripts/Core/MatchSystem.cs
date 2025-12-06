using System;
using System.Collections;
using UnityEngine;

public class MatchSystem
{
    private CardBehaviour firstCard;
    private CardBehaviour secondCard;

    private float flipBackDelay = .4f;

    public event Action<CardBehaviour, CardBehaviour> OnMatchFound;
    public event Action<CardBehaviour, CardBehaviour> OnMismatch;
    public event Action OnAllPairsCompleted;

    private int totalPairs;
    private int matchedPairs;

    public bool IsAnimating { get; private set; }

    public void Initialize(in int numberOfCards)
    {
        matchedPairs = 0;
        totalPairs = numberOfCards / 2;

        firstCard = null;
        secondCard = null;
        IsAnimating = false;
    }

    public void HandleCardSelected(CardBehaviour card)
    {

        if (card == firstCard)
        {
            firstCard.ShowBack();
            firstCard = null;
            return;
        }

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

        if (firstCard.CardData == secondCard.CardData)
        {
            matchedPairs++;
            OnMatchFound?.Invoke(firstCard, secondCard);

            GameManager.Instance.StartRoutine(HideCardCoroutine());

            if (matchedPairs >= totalPairs)
                OnAllPairsCompleted?.Invoke();
        }
        else
        {
            OnMismatch?.Invoke(firstCard, secondCard);
            GameManager.Instance.StartRoutine(FlipBackCoroutine());
        }
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
