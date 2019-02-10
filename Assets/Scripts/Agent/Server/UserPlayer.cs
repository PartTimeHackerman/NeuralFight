public class UserPlayer
{
    public string UserName;
    public string PlayerID;
    public long Rank;
    public long Score;

    public UserPlayer(string userName, string playerId, long rank, long score)
    {
        UserName = userName;
        PlayerID = playerId;
        Rank = rank;
        Score = score;
    }
}