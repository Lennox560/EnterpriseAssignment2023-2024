using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket
    {
        //• Ticket.cs – Id, Row, Column, FlightIdFK, Passport, PricePaid, Cancelled
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Row cannot be left blank")]
        public int Row {  get; set; }

        [Required(ErrorMessage = "Column cannot be left blank")]
        public int Column { get; set; }

        [ForeignKey("Flight")]
        public int FlightIdFk { get; set; }
        public virtual Flight Flight { get; set; }

        [Required(ErrorMessage = "Passport cannot be left blank")]
        public string Passport { get; set; }

        [Required(ErrorMessage = "PricePaid cannot be left blank")]
        public decimal PricePaid { get; set; }
        public bool Cancelled { get; set; }
        
        public string? PassportImage { get; set; }

    }
}
