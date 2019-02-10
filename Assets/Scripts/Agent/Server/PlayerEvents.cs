using UnityEngine;

public static class PlayerEvents
{
    public static void MatchEnd(UserPlayer winner, UserPlayer loser, int winnerPoints, int loserPoints)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("MatchEnd")
            .SetEventAttribute("WinnerID", winner.PlayerID)
            .SetEventAttribute("LoserID", loser.PlayerID)
            .SetEventAttribute("WinnerScore", winnerPoints)
            .SetEventAttribute("LoserScore", loserPoints)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                }
                else
                {
                    Debug.Log("Error saving score");
                }
            });
    }
}