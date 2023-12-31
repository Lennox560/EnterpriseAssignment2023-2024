using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private FlightsRepository _FlightsRepository;
        private TicketsRepository _TicketsRepository;
        //
        public TicketsController(FlightsRepository FlightsRepository, TicketsRepository TicketsRepository)
        {
            _FlightsRepository = FlightsRepository;
            _TicketsRepository = TicketsRepository;
        }


        public IActionResult Index()
        {
            var currentDate = DateTime.Now;

            IQueryable<Flight> list = _FlightsRepository.GetFlights().Where(f=> f.DepartureDate >= currentDate);
            
            

            var output = from f in list
                         select new ListFlightViewModel()
                         {
                             Id = f.Id,
                             CountryFrom = f.CountryFrom,
                             CountryTo = f.CountryTo,
                             DepartureDate = f.DepartureDate,
                             ArrivalDate = f.ArrivalDate,
                             RetailPrice = f.WholesalePrice * 1.2m,
                             SeatsAvailable = f.Columns*f.Rows - (_TicketsRepository.GetTickets().Where(x=>x.FlightIdFk == f.Id && !x.Cancelled).Count())
                         };
            return View(output);
        }

        [HttpGet]
        public IActionResult Book()
        {

        }

        
    }
}
