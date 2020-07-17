using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Models;


namespace WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservationController : ControllerBase
    {
        private Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationController(Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetReservation")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation()
        {

            return await _context.Reservations.ToListAsync();

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }


       
        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> Reservation([FromBody] Reservation reservation )
         {
           
            if (!ModelState.IsValid)
                return BadRequest("Not a vailable");
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

             return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
          // return Ok;
        }


    }
}
