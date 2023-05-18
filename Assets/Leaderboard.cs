using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

public class Leaderboard : MonoBehaviour
{
    public string leaderboardId = "Points";
    public string playerId;
    
    public string playerName;
    public double playerScore;
    
    private IAuthenticationService authService;
    private ILeaderboardsService leaderboard;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        authService = AuthenticationService.Instance;
        leaderboard = LeaderboardsService.Instance;

        authService.SignedIn += () =>
        {
            playerId = authService.PlayerId;

            Debug.Log("Signed in as: " + playerId + " :: " + playerName);
        };

        authService.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log("PROBLEM: " + s);
        };

        await authService.SignInAnonymouslyAsync();
    }

    private async void Init()
    {
        var playerEntry = await GetPlayerScore();
        if (playerEntry == null)
        {
            playerEntry = await SetPlayerScore(0);
        }
        playerName = playerEntry.PlayerName;
        playerScore = playerEntry.Score;

        await GetHighScores();
    }

    public async Task<LeaderboardEntry> GetPlayerScore()
    {
        try
        {
            var scoreResult = await leaderboard.GetPlayerScoreAsync(leaderboardId);
            return scoreResult;
        }
        catch (LeaderboardsException e)
        {
            if (e.Reason == LeaderboardsExceptionReason.EntryNotFound)
            {
                return null;
            }
            else
            {
                throw e;
            }
        }
    }

    public async Task<LeaderboardEntry> SetPlayerScore(double score)
    {
        var playerEntry = await leaderboard.AddPlayerScoreAsync(leaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
        return playerEntry;
    }

    public async Task<LeaderboardScoresPage> GetHighScores()
    {
        var scoresResponse = await leaderboard.GetScoresAsync(leaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        
        return scoresResponse;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

}
