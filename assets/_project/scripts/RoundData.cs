using Map;

[System.Serializable]
public struct RoundData
{
    public RoundData(int playersNum, int skippsNum, GameMap map)
    {
        players = new player[playersNum];
        for (int i = 0; i < players.Length; i++)
        {
            players[i].id = i;
            players[i].RemainingSkips = skippsNum;
        }
        this.map = map;
        turn = -1;
        isStarted = false;
        roundState = RoundState.loading;
        diceValue = -1;
    }
    public player[] players;
    public GameMap map;
    public int turn;
    public bool isStarted;
    public RoundState roundState;
    public int diceValue;
}

public enum RoundState
{
    loading, idle, Dice, movement
}