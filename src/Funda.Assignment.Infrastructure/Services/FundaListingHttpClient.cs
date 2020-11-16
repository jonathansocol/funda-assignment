using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Funda.Assignment.Application.Interfaces;
using Funda.Assignment.Domain.Entities;
using Funda.Assignment.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace Funda.Assignment.Infrastructure.Services
{
    public class FundaListingHttpClient : IListingHttpClient
    {
        public readonly HttpClient _httpClient;
        public readonly ILogger<FundaListingHttpClient> _logger;
        
        public FundaListingHttpClient(HttpClient httpClient, ILogger<FundaListingHttpClient> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<ListingObject>> GetListing(string filterCriteria = "")
        {
            filterCriteria = string.IsNullOrEmpty(filterCriteria) ? "" : $"/{filterCriteria}";

            var currentPage = 1;
            long totalPages = 0;

            var objectsList = new List<ApiObject>();

            while (totalPages == 0 || currentPage <= totalPages)
            {
                try
                {
                    var results = await _httpClient.GetAsync($"?type=koop&zo=/amsterdam{filterCriteria}/&page={currentPage}&pagesize=25");

                    if (results.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Request rate limit has been reached, API key may be shared across applications
                        await Task.Delay(5000);

                        _logger.LogInformation("Requests rate limit reached, waiting...");

                        continue;
                    }

                    results.EnsureSuccessStatusCode();
                    
                    var apiResponse = await results.Content.ReadAsAsync<ApiResponse>();

                    if (currentPage == 1)
                    {
                        totalPages = apiResponse.Paging.AantalPaginas;
                    }

                    objectsList.AddRange(apiResponse.Objects);

                    currentPage++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);

                    throw ex;
                }
            }

            var listingObjects = objectsList
                .Select(x => new ListingObject { AgentName = x.MakelaarNaam })
                .ToList();

            return listingObjects;
        }
    }
}
