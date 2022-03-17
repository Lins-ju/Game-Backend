using backend.Models;
using Microsoft.AspNetCore.Cors;
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

        public async Task<IActionResult> ShowFormation(string track, string name, long time)
        {
            _leaderboards.addLeaderboard(track, name, time);
            return Ok();
        }

        [HttpGet]

        public async Task<IActionResult> GetForm()
        {
            return Ok(_leaderboards.Leaderboards);
        }
    }
}