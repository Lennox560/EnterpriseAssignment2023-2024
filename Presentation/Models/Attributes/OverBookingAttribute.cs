using Presentation.Models.ViewModels;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Models.Attributes
{
    public class OverBookingAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (ListFlightViewModel)validationContext.ObjectInstance;
            // Implement your custom logic to prevent overbooking here
            // For instance, check against the available seats, booked seats, etc.

            // For example:
            if (viewModel.BookedSeats > viewModel.TotalSeats)
            {
                return new ValidationResult("Overbooking not allowed.");
            }

            return ValidationResult.Success;
        }

    }
}
