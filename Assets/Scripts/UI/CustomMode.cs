using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages custom game mode settings and validation.
/// </summary>
public class CustomMode : MonoBehaviour
{
    [Header("Custom Mode UI")]
    [Tooltip("Text displaying selected rows count.")]
    [SerializeField] private TMP_Text rowsTxt;
    
    [Tooltip("Text displaying selected columns count.")]
    [SerializeField] private TMP_Text columnsTxt;
    
    [Tooltip("Error message for invalid configurations.")]
    [SerializeField] private GameObject errorTxt;

    [Tooltip("Toggle for selecting to start a game in Blind Mode.")]
    [SerializeField] private Toggle toggleBlindMode;

    /// <summary>
    /// Coroutine for displaying error messages.
    /// </summary>
    private Coroutine errorCoroutine;
    
    /// <summary>
    /// Currently selected number of rows.
    /// </summary>
    private int selectedRows = 2;
    
    /// <summary>
    /// Currently selected number of columns.
    /// </summary>
    private int selectedColumns = 2;

    /// <summary>
    /// Minimum allowed grid dimension.
    /// </summary>
    private const int MIN_VALUE = 2;
    
    /// <summary>
    /// Maximum allowed grid dimension.
    /// </summary>
    private const int MAX_VALUE = 6;

    /// <summary>
    /// Initializes UI with default values.
    /// </summary>
    private void Start()
    {
        rowsTxt.text = selectedRows.ToString();
        columnsTxt.text = selectedColumns.ToString();
    }
    
    /// <summary>
    /// Updates UI text elements with current values.
    /// </summary>
    private void UpdateUI()
    {
        rowsTxt.text = selectedRows.ToString();
        columnsTxt.text = selectedColumns.ToString();
    }

    /// <summary>
    /// Increases the selected rows count.
    /// </summary>
    public void IncreaseRows()
    {
        selectedRows = Mathf.Clamp(selectedRows + 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    /// <summary>
    /// Increases the selected columns count.
    /// </summary>
    public void IncreaseColumns()
    {
        selectedColumns = Mathf.Clamp(selectedColumns + 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    /// <summary>
    /// Decreases the selected rows count.
    /// </summary>
    public void DecreaseRows()
    {
        selectedRows = Mathf.Clamp(selectedRows - 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    /// <summary>
    /// Decreases the selected columns count.
    /// </summary>
    public void DecreaseColumns()
    {
        selectedColumns = Mathf.Clamp(selectedColumns - 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    /// <summary>
    /// Validates and starts a custom game with selected dimensions.
    /// </summary>
    public void OnCustomSelection()
    {
        if (selectedRows * selectedColumns % 2 != 0)
        {
            if (errorCoroutine != null)
                StopCoroutine(errorCoroutine);
            errorCoroutine = StartCoroutine(ErrorCoroutine());
            return;
        }
        GameBoot.IsNewGame = true;
        GameBoot.IsBlindMode = toggleBlindMode.isOn;
        GameBoot.Rows = selectedRows;
        GameBoot.Columns = selectedColumns;
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Coroutine to display error message temporarily.
    /// </summary>
    private IEnumerator ErrorCoroutine()
    {
        errorTxt.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        errorTxt.SetActive(false);
    }
}