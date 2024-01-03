﻿using DataAccess.Context;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketsRepository : ITickets
    {
        private AirlineDbContext _airlineDbContext;

        public TicketsRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext;
        }
        
        public bool Book(Ticket t)
        {
            var existingTicket = GetTickets().SingleOrDefault(x=>x.FlightIdFk == t.FlightIdFk
            && x.Column == t.Column
            && x.Row == t.Row
            && !x.Cancelled);

            if(existingTicket != null)
            {
                //This means that a ticket already exists for this seat on the flight
                return false;
            }

            _airlineDbContext.Tickets.Add(t);
            _airlineDbContext.SaveChanges();
            return true;
        } 

        public void Cancel(Guid t)
        {
            var ticket = GetTickets().SingleOrDefault(x => x.Id == t);
            if(ticket != null)
            {
                ticket.Cancelled = true;
                _airlineDbContext.SaveChanges();
            }
        }

        public IQueryable<Ticket> GetTickets()
        {
            return _airlineDbContext.Tickets;
        }

        public int GetSeatAmount(int t)
        {
            return GetTickets().Where(x => x.FlightIdFk == t && !x.Cancelled).Count();
        }
    }
}
