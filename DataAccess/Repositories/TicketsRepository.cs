using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketsRepository
    {
        private AirlineDbContext _airlineDbContext;

        public TicketsRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }
        
        public void Book(Ticket t)
        {
            _airlineDbContext.Tickets.Add(t);
            _airlineDbContext.SaveChanges();
        } 

        public void Cancel(Ticket t)
        {
            _airlineDbContext.Tickets.Remove(t);
            _airlineDbContext.SaveChanges();
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _airlineDbContext.Tickets;
        }
    }
}
