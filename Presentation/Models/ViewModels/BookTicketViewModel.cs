using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class BookTicketViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int FlightId { get; set; }
        public string Passport { get; set; }
        public decimal PricePaid { get; set; }
        public bool Cancelled { get; set; }
        public IFormFile PassportImageFile { get; set; }

    }
}
