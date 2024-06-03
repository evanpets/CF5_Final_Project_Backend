namespace FinalProjectAPIBackend.Data
{
    public class VenueAddress
    {
        public int VenueAddressId { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber {  get; set; }
        public string? ZipCode {  get; set; }
        public virtual Venue? Venue { get; set; }
    }
}
