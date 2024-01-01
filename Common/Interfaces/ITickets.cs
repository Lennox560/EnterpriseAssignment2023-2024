using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITickets
    {
        bool Book(Ticket t);
        void Cancel(Ticket t);

        IQueryable<Ticket> GetTickets();

    }
}
