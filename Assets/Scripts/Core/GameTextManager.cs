using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text turnText;

    public void UpdatePointsText(in int match)
    {
        pointsText.text = "Points : " + match;
    }

    public void UpdateTurnText(in int turn)
    {
        turnText.text = "Turn : " + turn;
    }
}
