﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.Data
{
    public class VenueAddress
    {
        [Key]
        public int VenueAddressId { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber {  get; set; }
        public string? City { get; set; }
        public string? ZipCode {  get; set; }
        [JsonIgnore]
        public virtual Venue? Venue { get; set; }
    }
}
