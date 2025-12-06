using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomMode : MonoBehaviour
{
    [SerializeField] private TMP_Text rowsTxt;
    [SerializeField] private TMP_Text columnsTxt;
    [SerializeField] private GameObject errorTxt;

    private Coroutine errorCoroutine;
    private int selectedRows = 2;
    private int selectedColumns = 2;

    private const int MIN_VALUE = 2;
    private const int MAX_VALUE = 6;

    private void Start()
    {
        rowsTxt.text = selectedRows.ToString();
        columnsTxt.text = selectedColumns.ToString();
    }
    private void UpdateUI()
    {
        rowsTxt.text = selectedRows.ToString();
        columnsTxt.text = selectedColumns.ToString();
    }

    public void IncreaseRows()
    {
        selectedRows = Mathf.Clamp(selectedRows + 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    public void IncreaseColumns()
    {
        selectedColumns = Mathf.Clamp(selectedColumns + 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    public void DecreaseRows()
    {
        selectedRows = Mathf.Clamp(selectedRows - 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }

    public void DecreaseColumns()
    {
        selectedColumns = Mathf.Clamp(selectedColumns - 1, MIN_VALUE, MAX_VALUE);
        UpdateUI();
    }


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
        GameBoot.Rows = selectedRows;
        GameBoot.Columns = selectedColumns;
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator ErrorCoroutine()
    {
        errorTxt.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        errorTxt.SetActive(false);

    }
}
