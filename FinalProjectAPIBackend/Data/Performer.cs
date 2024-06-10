﻿using FinalProjectAPIBackend.Models;
using Newtonsoft.Json;

namespace FinalProjectAPIBackend.Data
{
    public class Performer
    {
        public int PerformerId { get; set; }
        public string? Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    };
}
