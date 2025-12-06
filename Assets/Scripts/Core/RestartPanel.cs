using UnityEngine;

public class RestartPanel : MonoBehaviour
{
    [SerializeField] private GameObject restartPanel;
    private void OnEnable()
    {
        GameManager.Instance.GameEnded += ShowRestartPanel;
    }

    public void ShowRestartPanel()
    {
        restartPanel.SetActive(!restartPanel.activeInHierarchy);
    }
    
}
