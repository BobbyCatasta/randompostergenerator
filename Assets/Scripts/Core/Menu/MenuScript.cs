using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] Button loadGameBtn;
    [SerializeField] GameObject customCanvas;
    [SerializeField] GameObject mainMenuCanvas;
    void Start()
    {
        if (SaveSystem.LoadGame() == null)
            loadGameBtn.interactable = false;
    }
    
    public void LoadGame()
    {
        GameBoot.IsNewGame = false;
        SceneManager.LoadScene("GameScene");
    }

    public void OnEasySelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 3;
        GameBoot.Columns = 4;
        LoadGame();
    }

    public void OnMediumSelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 4;
        GameBoot.Columns = 5;
        LoadGame();
        
    }

    public void OnHardSelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 6;
        GameBoot.Columns = 6;
        LoadGame();
    }


    public void ToggleCustomMode()
    {
        customCanvas.SetActive(!customCanvas.activeInHierarchy);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
