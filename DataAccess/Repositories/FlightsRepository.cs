using DataAccess.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FlightsRepository
    {
        private AirlineDbContext _airlineDbContext;

        public FlightsRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }

        public IQueryable<Flight> GetFlights()
        {
            return _airlineDbContext.Flights;
        }

        public Flight? GetFlight(int id)
        {
            return GetFlights().SingleOrDefault(x => x.Id == id);
        }


    }
}
