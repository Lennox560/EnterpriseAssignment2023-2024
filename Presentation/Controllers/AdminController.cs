using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private FlightsRepository _FlightsRepository;
        private ITickets _TicketsRepository;

        public AdminController(FlightsRepository FlightsRepository, ITickets TicketsRepository)
        {
            _FlightsRepository = FlightsRepository;
            _TicketsRepository = TicketsRepository;
        }

        public IActionResult Index()
        {
            IQueryable<Flight> list = _FlightsRepository.GetFlights();



            var output = from f in list
                         select new ListFlightViewModel()
                         {
                             Id = f.Id,
                             CountryFrom = f.CountryFrom,
                             CountryTo = f.CountryTo,
                             DepartureDate = f.DepartureDate,
                             ArrivalDate = f.ArrivalDate,
                             RetailPrice = f.WholesalePrice * f.CommissionRate,
                             SeatsAvailable = f.Columns * f.Rows - _TicketsRepository.GetSeatAmount(f.Id)
                         };
            return View(output);
        }

        public IActionResult Show(int id)
        {

            IQueryable<Ticket> list = _TicketsRepository.GetTickets().Where(x=>x.FlightIdFk == id) ;



            var output = from t in list
                         select new ListTicketViewModel()
                         {
                             Id = t.Id,
                             CountryFrom = _FlightsRepository.GetFlight(t.FlightIdFk).CountryFrom,
                             CountryTo = _FlightsRepository.GetFlight(t.FlightIdFk).CountryTo,
                             DepartureDate = _FlightsRepository.GetFlight(t.FlightIdFk).DepartureDate,
                             ArrivalDate = _FlightsRepository.GetFlight(t.FlightIdFk).ArrivalDate,
                             Passport = t.Passport,
                             FlightIdFk = t.FlightIdFk,
                             PricePaid = t.PricePaid,
                             Column = t.Column,
                             Row = t.Row,
                             Cancelled = t.Cancelled
                         };
            return View(output);
        }

        public IActionResult Details(Guid id)
        {
            var t = _TicketsRepository.GetTickets().SingleOrDefault(x=> x.Id == id);
            if (t == null)
            {
                TempData["error"] = "No Ticket found";
                return RedirectToAction("show/");
            }
            else
            {
                TicketViewModel myTicket = new TicketViewModel()
                {
                    Id = t.Id,
                    CountryFrom = _FlightsRepository.GetFlight(t.FlightIdFk).CountryFrom,
                    CountryTo = _FlightsRepository.GetFlight(t.FlightIdFk).CountryTo,
                    DepartureDate = _FlightsRepository.GetFlight(t.FlightIdFk).DepartureDate,
                    ArrivalDate = _FlightsRepository.GetFlight(t.FlightIdFk).ArrivalDate,
                    Passport = t.Passport,
                    FlightIdFk = t.FlightIdFk,
                    PricePaid = t.PricePaid,
                    Column = t.Column,
                    Row = t.Row,
                    Cancelled = t.Cancelled,
                    PassportImage = t.PassportImage
                    
                };


                return View(myTicket);
            }
        }

    }
}
