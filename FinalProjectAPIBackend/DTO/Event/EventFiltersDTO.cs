using FinalProjectAPIBackend.DTO.Performer;

namespace FinalProjectAPIBackend.DTO.Event
{
    public class EventFiltersDTO
    {
        public string? Title { get; set; }
        public int? VenueId { get; set; }
        public decimal? Price { get; set; }
        public List<PerformerReadOnlyDTO>? Performers { get; set; }
        public DateOnly? Date { get; set; }

    }
}
