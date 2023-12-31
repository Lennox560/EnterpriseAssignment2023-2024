﻿using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModels;
using System.IO;

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
                             RetailPrice = f.WholesalePrice * f.CommissionRate,
                             SeatsAvailable = f.Columns*f.Rows - (_TicketsRepository.GetTickets().Where(x=>x.FlightIdFk == f.Id && !x.Cancelled).Count())
                         };
            return View(output);
        }

        
        [HttpGet]
        public IActionResult Book(int flight, decimal price)
        {
            BookTicketViewModel myModel = new BookTicketViewModel
            {
                FlightId = flight,
                PricePaid = price
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

        public IActionResult Tickets()
        {

            IQueryable<Ticket> list = _TicketsRepository.GetTickets();
            //let to be set to get the tickets of the logged in user

            var output = from t in list
                         select new ListTicketViewModel()
                         {
                             Id = t.Id,
                             CountryFrom = t.Flight.CountryFrom,
                             CountryTo = t.Flight.CountryTo,
                             DepartureDate = t.Flight.DepartureDate,
                             ArrivalDate = t.Flight.ArrivalDate,
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
