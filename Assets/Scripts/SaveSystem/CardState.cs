using System;

[Serializable]
public struct CardState
{
    public string suitID;    
    public bool isMatched;
    public bool flippedByPlayer;
    public bool hasBeenSelected;
    

    public CardState(string suitID, bool flippedByPlayer, bool isMatched,bool hasBeenSelected)
    {
        this.suitID = suitID;
        this.flippedByPlayer = flippedByPlayer;
        this.isMatched = isMatched;
        this.hasBeenSelected = hasBeenSelected;
    }
}
