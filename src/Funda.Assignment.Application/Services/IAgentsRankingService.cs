using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Assignment.Application.Models;

namespace Funda.Assignment.Application.Services
{
    public interface IAgentsRankingService
    {
        Task<List<RankedAgentDto>> GetAgentsLeaderboard(int leaderboardSize, string filterCriteria = "");
    }
}