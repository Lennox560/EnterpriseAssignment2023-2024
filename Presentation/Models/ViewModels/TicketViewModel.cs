namespace Presentation.Models.ViewModels
{
    public class TicketViewModel
    {
        public Guid Id { get; set; }

        public string Passport { get; set; }

        public string CountryFrom { get; set; }

        public string CountryTo { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public int FlightIdFk { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public decimal PricePaid { get; set; }

        public bool Cancelled { get; set; }

        public string? PassportImage { get; set; }
    }
}
