[System.Serializable]
public struct player
{
  
    public player(int id, int skipsNum)
    {
        this.id = id;
        position = new Position(0, 0);
        RemainingSkips = skipsNum;
        lastRoll = -1;
        isSkippedLastTurn = false;
    }
    public int id;
    public Position position;
    public int RemainingSkips;
    public int lastRoll;
    public bool isSkippedLastTurn;
}