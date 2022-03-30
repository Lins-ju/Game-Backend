using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly GameLeaderboards _leaderboards;
        public SystemController(GameLeaderboards leaderboards)
        {
            _leaderboards = leaderboards;
        }

        [HttpPost]

        public async Task<IActionResult> ShowFormation(AddLeaderboardRequest addLeaderboardRequest)
        {
            _leaderboards.AddLeaderboard(addLeaderboardRequest.Track, addLeaderboardRequest.Name, addLeaderboardRequest.Time);
            return Ok();
        }

        [HttpGet]

        public async Task<IActionResult> GetForm()
        {
            return Ok(_leaderboards.Leaderboards);
        }
    }
}