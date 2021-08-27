using Map;

[System.Serializable]
public struct RoundData
{
    public RoundData(int playersNum, GameMap map)
    {
        players = new player[playersNum];
        this.map = map;
        turn = 0;
    }
    public player[] players;
    public GameMap map;
    public int turn;
}
