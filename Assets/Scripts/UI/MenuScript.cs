using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles main menu UI interactions and scene transitions.
/// </summary>
public class MenuScript : MonoBehaviour
{
    [Header("Menu UI Elements")]
    [Tooltip("Button for loading saved games.")]
    [SerializeField] Button loadGameBtn;
    
    [Tooltip("Canvas for custom game mode settings.")]
    [SerializeField] GameObject customCanvas;
    
    [Tooltip("Main menu canvas.")]
    [SerializeField] GameObject mainMenuCanvas;
    
    /// <summary>
    /// Initializes menu state on start.
    /// </summary>
    void Start()
    {
        if (SaveSystem.LoadGame() == null)
            loadGameBtn.interactable = false;
    }
    
    /// <summary>
    /// Loads a saved game.
    /// </summary>
    public void LoadGame()
    {
        GameBoot.IsNewGame = false;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Starts a new game in easy mode (3x4 grid).
    /// </summary>
    public void OnEasySelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 3;
        GameBoot.Columns = 4;
        GameBoot.IsBlindMode = false;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Starts a new game in medium mode (4x5 grid).
    /// </summary>
    public void OnMediumSelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 4;
        GameBoot.Columns = 5;
        GameBoot.IsBlindMode = false;
        SceneManager.LoadScene("GameScene");

    }

    /// <summary>
    /// Starts a new game in hard mode (6x6 grid).
    /// </summary>
    public void OnHardSelection()
    {
        GameBoot.IsNewGame = true;
        GameBoot.Rows = 6;
        GameBoot.Columns = 6;
        GameBoot.IsBlindMode = false;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Toggles between main menu and custom mode UI.
    /// </summary>
    public void ToggleCustomMode()
    {
        customCanvas.SetActive(!customCanvas.activeInHierarchy);
        mainMenuCanvas.SetActive(!mainMenuCanvas.activeInHierarchy);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}