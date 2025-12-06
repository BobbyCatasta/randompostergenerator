using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip cardFlip;
    [SerializeField] private AudioClip match;
    [SerializeField] private AudioClip mismatch;
    [SerializeField] private AudioClip gameOver;

    public void Play(SoundType type)
    {
        switch (type)
        {
            case SoundType.CardFlip:
                source.PlayOneShot(cardFlip);
                break;

            case SoundType.Match:
                source.PlayOneShot(match);
                break;

            case SoundType.Mismatch:
                source.PlayOneShot(mismatch);
                break;

            case SoundType.GameOver:
                source.PlayOneShot(gameOver);
                break;
        }
    }
}

public enum SoundType
{
    CardFlip,
    Match,
    Mismatch,
    GameOver
}
