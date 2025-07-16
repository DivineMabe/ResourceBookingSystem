using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }

        // Relationship to Bookings
        public List<Booking>? Bookings { get; set; }
    }
}
