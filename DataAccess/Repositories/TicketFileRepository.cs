using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketFileRepository : ITickets
    {
        string filePath;

        public TicketFileRepository(string pathToProductsFile)
        {
            filePath = pathToProductsFile;


            if (System.IO.File.Exists(filePath) == false)
            {
                using (var myFile = System.IO.File.Create(filePath))
                {
                    myFile.Close();
                }
            }
        }

        public bool Book(Ticket t)
        {
            t.Id = Guid.NewGuid();

            var existingTicket = GetTickets().SingleOrDefault(x => 
            x.FlightIdFk == t.FlightIdFk
            && x.Column == t.Column
            && x.Row == t.Row
            && !x.Cancelled);

            if (existingTicket != null)
            {
                //This means that a ticket already exists for this seat on the flight
                return false;
            }
            else
            {
                var myList = GetTickets().ToList();
                myList.Add(t);
                
                string jsonString = JsonSerializer.Serialize(myList);
                System.IO.File.WriteAllText(filePath, jsonString);
                return true;
            }
        }

        public void Cancel(Ticket t)
        {
            var myList = GetTickets();
            var toCancel = myList.FirstOrDefault(x=> x.Id == t.Id);
            if(toCancel != null)
            {
                toCancel.Cancelled = true;
            }
            string jsonString = JsonSerializer.Serialize(myList);
            System.IO.File.WriteAllText(filePath, jsonString);
        }

        public IQueryable<Ticket> GetTickets()
        {
            string allText = System.IO.File.ReadAllText(filePath);

            if (allText == "")
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                //note: next line will convert from normal text into json-formatted-object
                try
                {

                    List<Ticket> tickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                    return tickets.AsQueryable();
                }
                catch
                {
                    return new List<Ticket>().AsQueryable();
                }

            }
        }

        public int GetSeatAmount(int t)
        {
            return GetTickets().Where(x => x.FlightIdFk == t && !x.Cancelled).Count();
        }
    }
}
