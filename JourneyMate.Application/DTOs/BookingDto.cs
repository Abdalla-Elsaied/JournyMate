using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class BookingDto : IValidatableObject
    {
        [Required]
        public int TravelId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total quantity must be at least 1")]
        public int TotalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Children under five number cannot be negative")]
        public int ChildrenUnderFiveNum { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Custom validation method to ensure business rules
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Validate that TotalQuantity is at least 1
            if (TotalQuantity < 1)
            {
                results.Add(new ValidationResult(
                    "Total quantity must be at least 1.",
                    new[] { nameof(TotalQuantity) }));
            }

            // Validate that ChildrenUnderFiveNum is not greater than TotalQuantity
            if (ChildrenUnderFiveNum > TotalQuantity)
            {
                results.Add(new ValidationResult(
                    "Children under five cannot exceed total quantity.",
                    new[] { nameof(ChildrenUnderFiveNum) }));
            }

            // Validate that National ID is not empty
            if (string.IsNullOrWhiteSpace(NationalId))
            {
                results.Add(new ValidationResult(
                    "National ID is required.",
                    new[] { nameof(NationalId) }));
            }

            // Validate that Phone Number is not empty
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                results.Add(new ValidationResult(
                    "Phone number is required.",
                    new[] { nameof(PhoneNumber) }));
            }

            return results;
        }
    }
}