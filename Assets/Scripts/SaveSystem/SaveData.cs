using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int nRows;
    public int nColumns;

    public List<CardState> cardsState;

    public int turn;
    public int points;
    public int pointIncrease;
}
