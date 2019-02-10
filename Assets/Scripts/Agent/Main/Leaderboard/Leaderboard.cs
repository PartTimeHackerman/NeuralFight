using System.Collections.Generic;
using System.Linq;
using GameSparks.Api.Responses;
using Nakama.TinyJson;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public List<PlayerRecord> FirstRecords = new List<PlayerRecord>();
    public List<PlayerRecord> Records = new List<PlayerRecord>();

    public bool CanRetriveLeaderboard = false;
    private void Start()
    {
        Auth.OnAuth += (u, i) =>
        {
            CanRetriveLeaderboard = true;
            GetLeaderboard();
        };
    }

    public void GetLeaderboard()
    {
        if (!CanRetriveLeaderboard)
        {
            return;
        }
        new GameSparks.Api.Requests
                .AroundMeLeaderboardRequest()
            .SetLeaderboardShortCode("Leaderboard")
            .SetEntryCount(7)
            .SetIncludeFirst(3)
            .Send(response =>
            {
                AroundMeLeaderboardResponse._LeaderboardData[] firstsRecords = response.First.ToArray();
                AroundMeLeaderboardResponse._LeaderboardData[] records = response.Data.ToArray();


                for (var i = 0; i <= 2; i++)
                {
                    AroundMeLeaderboardResponse._LeaderboardData leaderboardData = firstsRecords[i];
                    long rank = leaderboardData.Rank.Value;
                    string playerName = leaderboardData.UserName;
                    string playerID = leaderboardData.UserId;
                    long score = leaderboardData.BaseData.GetLong("Scorer").Value;
                    UserPlayer userPlayer = new UserPlayer(playerName, playerID, rank, score);
                    FirstRecords[i].SetPlayer(userPlayer);
                }

                for (var i = 0; i <= 7; i++)
                {
                    AroundMeLeaderboardResponse._LeaderboardData leaderboardData = records[i];
                    long rank = leaderboardData.Rank.Value;
                    string playerName = leaderboardData.UserName;
                    string playerID = leaderboardData.UserId;
                    long score = leaderboardData.BaseData.GetLong("Scorer").Value;
                    UserPlayer userPlayer = new UserPlayer(playerName, playerID, rank, score);
                    if (playerID.Equals(Auth.UserId))
                    {
                        PlayerCollections.UserPlayer = userPlayer;
                    }
                    Records[i].SetPlayer(userPlayer);
                }
            });
    }
}