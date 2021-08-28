using Map;

[System.Serializable]
public struct RoundData
{
    public RoundData(int playersNum, int skippsNum, GameMap map)
    {
        players = new player[playersNum];
        for (int i = 0; i < players.Length; i++)
        {
            players[i].RemainingSkips = skippsNum;
        }
        this.map = map;
        turn = -1;
        isStarted = false;
        roundState = RoundState.idle;
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
    idle, Dice, movement 
}