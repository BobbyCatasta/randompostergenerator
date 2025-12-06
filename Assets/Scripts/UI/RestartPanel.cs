using UnityEngine;

public class RestartPanel : MonoBehaviour
{
    [SerializeField] private GameObject restartPanel;
    private void OnEnable()
    {
        GameEvents.GameEnded += ShowRestartPanel;
    }

    private void OnDisable()
    {
        GameEvents.GameEnded -= ShowRestartPanel;
    }
    public void ShowRestartPanel()
    {
        restartPanel.SetActive(!restartPanel.activeInHierarchy);
    }
    
}
