using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModels;
using System.IO;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private FlightsRepository _FlightsRepository;
        private ITickets _TicketsRepository;
        private readonly UserManager<User> _userManager;

        public TicketsController(FlightsRepository FlightsRepository, ITickets TicketsRepository, UserManager<User> userManager)
        {
            _FlightsRepository = FlightsRepository;
            _TicketsRepository = TicketsRepository;
            _userManager = userManager;
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
                             RetailPrice = f.WholesalePrice * f.CommissionRate,
                             SeatsAvailable = f.Columns * f.Rows - _TicketsRepository.GetSeatAmount(f.Id)
                         };
            return View(output);
        }

        
        [HttpGet]
        public async Task<IActionResult> Book(int flight, decimal price)
        {
            var user = await _userManager.GetUserAsync(User);
            string passportNumber = user?.passport;

            BookTicketViewModel myModel = new BookTicketViewModel
            {
                FlightId = flight,
                PricePaid = price,
                Passport = passportNumber,
                Row = _FlightsRepository.GetFlight(flight).Rows,
                Column = _FlightsRepository.GetFlight(flight).Columns

            };

            return View(myModel); 
        }

        [HttpPost]
        public IActionResult Book(BookTicketViewModel model, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                var currentDate = DateTime.Now;

                string relativePath = "";
                //upload of image
                
                if (model.PassportImageFile != null)
                {
                    //1. Generate a unique filename
                    string newFilename = Guid.NewGuid().ToString()
                        + Path.GetExtension(model.PassportImageFile.FileName);//.jpg

                    //2. Form the relative path
                    relativePath = "/images/" + newFilename;

                    //3. Form the absolute path
                    // to save the physical file //C:\..
                    string absolutePath = host.WebRootPath + "\\images\\" + newFilename;

                    //4. save the image in the folder
                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        model.PassportImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
                

                var existingTicket = _TicketsRepository.GetTickets().SingleOrDefault(x=>x.FlightIdFk == model.FlightId
                && x.Column == model.Column
                && x.Row == model.Row
                && !x.Cancelled);
                var flight = _FlightsRepository.GetFlight(model.FlightId);
                if (flight.DepartureDate > currentDate && existingTicket == null && model.Column > 0 && model.Row > 0) 
                {
                    _TicketsRepository.Book(new Ticket
                    {
                        Row = model.Row,
                        Column = model.Column,
                        FlightIdFk = model.FlightId,
                        Passport = model.Passport,
                        PricePaid = model.PricePaid,
                        PassportImage = relativePath,
                        Cancelled = false
                    });

                    TempData["message"] = "Ticket booked successfully!";

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
                
            }catch (Exception ex)
            {
                TempData["error"] = "Ticket not booked! Check you inputs";
                return View(model);
            }

        }

        [Authorize]
        public async Task<IActionResult> TicketsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            string passportNumber = user.passport;

            IQueryable<Ticket> list = _TicketsRepository.GetTickets().Where(x=> x.Passport == passportNumber);
            //let to be set to get the tickets of the logged in user

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

    }
}
