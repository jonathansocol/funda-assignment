using System.Collections.Generic;

namespace Funda.Assignment.Infrastructure.Models
{
    public class ApiResponse
    {
        public List<ApiObject> Objects { get; set; }
        public Paging Paging { get; set; }
        public long TotaalAantalObjecten { get; set; }
    }

    public class Metadata
    {
        public string ObjectType { get; set; }
        public string Omschrijving { get; set; }
        public string Titel { get; set; }
    }

    public class ApiObject
    {
        public long MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
    }

    public class Paging
    {
        public long AantalPaginas { get; set; }
        public long HuidigePagina { get; set; }
        public string VolgendeUrl { get; set; }
        public object VorigeUrl { get; set; }
    }
}
