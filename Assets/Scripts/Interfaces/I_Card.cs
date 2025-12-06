using UnityEngine;

public interface I_Card
{

    public string ID { get; }
    public void ShowFront(bool flippedByPlayer);
    public void ShowBack();
    public void DisableCard();
    public void HideCard();
    public void EnableCard();
    public void SetMatched();
    public void ResetCard();
    public void ShowMatchFeedback();
    public void ShowMismatchFeedback();
}
