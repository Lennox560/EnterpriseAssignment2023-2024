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
        void Cancel(Guid t);

        IQueryable<Ticket> GetTickets();

        int GetSeatAmount(int t);

    }
}
