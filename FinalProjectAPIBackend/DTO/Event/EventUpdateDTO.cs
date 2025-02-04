﻿using FinalProjectAPIBackend.DTO.Performer;
using FinalProjectAPIBackend.DTO.Venue;
using FinalProjectAPIBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectAPIBackend.DTO.Event
{
    public class EventUpdateDTO
    {
        [Required]
        [Editable(false)]
        public int EventId { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Event title should consist of at least 5 characters.")]
        [Required(ErrorMessage = "The event title is a required field.")]
        public string? Title { get; set; }

        [StringLength(250, ErrorMessage = "Description should not exceed 250 characters.")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "A valid date must be selected.")]
        public DateOnly? Date { get; set; }
        public VenueUpdateDTO? Venue { get; set; }
        public decimal? Price { get; set; }
        public ICollection<PerformerUpdateDTO>? Performers { get; set; }
        [Required(ErrorMessage = "An event category must be selected.")]
        public EventCategory? Category { get; set; }
        public int? UserId { get; set; }
        public string? ImageUrl { get; set; }


    }
}
