using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Front_End.Models
{
    public class ReservationViewModel
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter number only .")]
        public string GuestsNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public string Menu { get; set; }

        [StringLength(300, ErrorMessage = "The notes cannot been more than  300 characters. ")]
        public string Notes { get; set; }
        [Required]
        public string GuestName { get; set; }
        [NotMapped]
        public List<SelectListItem> MenuList { get; } = new List<SelectListItem>
        {
           new SelectListItem() { Text = "Breakfast", Value = "breakfast" },
           new SelectListItem() { Text = "Lunch", Value = "lunch" },
           new SelectListItem() { Text = "Dinner", Value = "dinner" },


        };
    }
}
