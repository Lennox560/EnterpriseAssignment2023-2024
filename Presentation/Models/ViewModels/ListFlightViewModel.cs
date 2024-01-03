using Presentation.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class ListFlightViewModel
    {
        public int Id { get; set; }

        //public int SeatsAvailable { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public decimal RetailPrice { get; set; }

        [OverBooking(ErrorMessage = "Overbooking not allowed.")]
        public int BookedSeats { get; set; }

        public int TotalSeats { get; set; }
    }
}
