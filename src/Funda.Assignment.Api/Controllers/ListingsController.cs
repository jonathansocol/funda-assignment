using System.Threading.Tasks;
using Funda.Assignment.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Funda.Assignment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IAgentsRankingService _agentsRankingService;

        public ListingsController(IAgentsRankingService agentsRankingService)
        {
            _agentsRankingService = agentsRankingService;
        }

        [HttpGet("GetTopAgent")]
        public async Task<ActionResult> GetTopAgent()
        {
            var results = await _agentsRankingService.GetAgentsLeaderboard(1);

            return Ok(results);
        }

        [HttpGet("GetAgentsLeaderboard")]
        public async Task<ActionResult> GetAgentsLeaderboard()
        {
            var results = await _agentsRankingService.GetAgentsLeaderboard(10);

            return Ok(results);
        }

        [HttpGet("GetAgentsLeaderboardForPropertiesWithGarden")]
        public async Task<ActionResult> GetAgentsLeaderboardForPropertiesWithGarden()
        {
            var results = await _agentsRankingService.GetAgentsLeaderboard(10, "tuin");

            return Ok(results);
        }
    }
}   
