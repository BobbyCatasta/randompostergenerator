using UnityEngine;

/// <summary>
/// Manages audio playback for game sounds.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Components")]
    [Tooltip("AudioSource component for playing sounds.")]
    [SerializeField] private AudioSource source;
    
    [Header("Sound Clips")]
    [Tooltip("Sound played when flipping a card.")]
    [SerializeField] private AudioClip cardFlip;
    
    [Tooltip("Sound played when cards match.")]
    [SerializeField] private AudioClip match;
    
    [Tooltip("Sound played when cards don't match.")]
    [SerializeField] private AudioClip mismatch;
    
    [Tooltip("Sound played when game ends.")]
    [SerializeField] private AudioClip gameOver;

    /// <summary>
    /// Plays a sound based on the specified type.
    /// </summary>
    /// <param name="type">Type of sound to play.</param>
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

/// <summary>
/// Enumeration of available game sounds.
/// </summary>
public enum MatchingCardsSound
{
    /// <summary>
    /// Sound when a card is flipped.
    /// </summary>
    CardFlip,
    
    /// <summary>
    /// Sound when two cards match.
    /// </summary>
    Match,
    
    /// <summary>
    /// Sound when two cards don't match.
    /// </summary>
    Mismatch,
    
    /// <summary>
    /// Sound when the game ends.
    /// </summary>
    GameOver
}