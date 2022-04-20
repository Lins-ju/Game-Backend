using Backend.Domain;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private readonly Domain.LeaderboardService leaderboardService;

        public LeaderboardController(LeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }


        [Route("save")]
        [HttpPost]

        public void SaveScore(EntryKey entryKey, LeaderboardEntry leaderboard)
        {
            leaderboardService.SaveScore(entryKey.TrackName, leaderboard.UserId, leaderboard.Score);
        }


        [Route("get")]
        [HttpGet]

        public async Task<Leaderboard> GetScores(string trackName)
        {
            return await leaderboardService.GetScores(trackName);
        } 
    }
}