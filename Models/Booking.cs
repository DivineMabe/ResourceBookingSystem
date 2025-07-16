using System;
using System.ComponentModel.DataAnnotations;

namespace ResourceBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [CustomValidation(typeof(Booking), nameof(ValidateEndTime))]
        public DateTime EndTime { get; set; }

        [Required]
        public string BookedBy { get; set; } = string.Empty;

        [Required]
        public string Purpose { get; set; } = string.Empty;

        public Resource? Resource { get; set; }

        public static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
        {
            var instance = (Booking)context.ObjectInstance;
            if (endTime <= instance.StartTime)
            {
                return new ValidationResult("End time must be after start time.");
            }
            return ValidationResult.Success;
        }
    }
}
