using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Flight
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Rows cannot be left blank")]
        public int Rows { get; set; }
        [Required(ErrorMessage = "Columns cannot be left blank")]
        public int Columns { get; set; }
        [Required(ErrorMessage = "DepartureDate cannot be left blank")]
        public DateTime DepartureDate { get; set; }
        [Required(ErrorMessage = "ArrivalDate cannot be left blank")]
        public DateTime ArrivalDate { get; set; }
        [Required(ErrorMessage = "CountryFrom cannot be left blank")]
        public string CountryFrom { get; set; }
        [Required(ErrorMessage = "CountryTo cannot be left blank")]
        public string CountryTo { get; set; }
        [Required(ErrorMessage = "WholesalePrice cannot be left blank")]
        public decimal WholesalePrice { get; set; }
        [Required(ErrorMessage = "CommissionRate cannot be left blank")]
        public decimal CommissionRate { get; set; }


    }
}
