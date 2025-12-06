using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip match;
    [SerializeField] private AudioClip mismatch;
    [SerializeField] private AudioClip gameOver;

    public void PlaySound(MatchingCardsSound type)
    {
        switch (type)
        {
            case MatchingCardsSound.CardFlip:
                source.PlayOneShot(cardFlip);
                break;

            case MatchingCardsSound.Match:
                source.PlayOneShot(match);
                break;

            case MatchingCardsSound.Mismatch:
                source.PlayOneShot(mismatch);
                break;

            case MatchingCardsSound.GameOver:
                source.PlayOneShot(gameOver);
                break;
        }
    }
}

public enum MatchingCardsSound
{
    CardFlip,
    Match,
    Mismatch,
    GameOver
}
