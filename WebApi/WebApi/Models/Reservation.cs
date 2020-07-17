using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public string GuestsNumber { get; set; }

        public DateTime Date { get; set; }

        public string Menu { get; set; }

        public string Notes { get; set; }

        public string GuestName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

    }
}
