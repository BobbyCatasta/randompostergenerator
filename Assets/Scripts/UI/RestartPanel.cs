using UnityEngine;

/// <summary>
/// Controls the restart panel UI that appears when game ends.
/// </summary>
public class RestartPanel : MonoBehaviour
{
    [Tooltip("The restart panel game object.")]
    [SerializeField] private GameObject restartPanel;
    
    /// <summary>
    /// Subscribes to game events.
    /// </summary>
    private void OnEnable()
    {
        GameEvents.GameEnded += ShowRestartPanel;
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDisable()
    {
        GameEvents.GameEnded -= ShowRestartPanel;
    }
    
    /// <summary>
    /// Toggles the visibility of the restart panel.
    /// </summary>
    public void ShowRestartPanel()
    {
        restartPanel.SetActive(!restartPanel.activeInHierarchy);
    }
}