using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Assignment.Domain.Entities;

namespace Funda.Assignment.Application.Interfaces
{
    public interface IListingHttpClient
    {
        Task<List<ListingObject>> GetListing(string filterCriteria);
    }
}