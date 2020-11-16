using System.Linq;
using System.Collections.Generic;
using Funda.Assignment.Application.Interfaces;
using Funda.Assignment.Application.Models;
using System.Threading.Tasks;

namespace Funda.Assignment.Application.Services
{
    public class AgentsRankingService : IAgentsRankingService
    {
        private readonly IListingHttpClient _client;

        public AgentsRankingService(IListingHttpClient client)
        {
            _client = client;
        }

        public async Task<List<RankedAgentDto>> GetAgentsLeaderboard(int leaderboardSize, string filterCriteria = "")
        {
            var listings = await _client.GetListing(filterCriteria);

            var rankedAgents = listings
                .GroupBy(listing => listing.AgentName)
                .OrderByDescending(x => x.Count())
                .Take(leaderboardSize)
                .Select(x => new RankedAgentDto { AgentName = x.Key, ListedObjectsCount = x.Count() })
                .ToList();

            return rankedAgents;
        }
    }
}