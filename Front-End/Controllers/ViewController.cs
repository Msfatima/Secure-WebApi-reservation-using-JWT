using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Front_End.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Authentication;
using WebApi.Data;

namespace Front_End.Controllers
{
    public class ViewController : Controller
    {
        private readonly ILogger<ViewController> _logger;
        private readonly IConfiguration _configuration;
     
        GuestApi _guestApi = new GuestApi();
       

        public ViewController(ILogger<ViewController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
           
        }
        //  login Page
        [HttpGet]
        public ActionResult Index()
        {
           
            return View();
        }

        //  authenticate guest login   
        [HttpPost]
        public async Task<IActionResult> Index(Login model, string returnUrl)
        {
            using (HttpClient client = _guestApi.Initial())
            {
                var content = new StringContent(JsonConvert.SerializeObject(model),
                     Encoding.UTF8, MediaTypeNames.Application.Json);

                using (var Response = await client.PostAsync("api/Authentication/Login", content))
                {
                    string stringJWT = Response.Content.ReadAsStringAsync().Result;
                    GuestApi jwt = JsonConvert.DeserializeObject<GuestApi>(stringJWT);

                    HttpContext.Session.SetString("token", jwt.Token);

                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("CreateReservation", returnUrl);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }
        }
       

        
        [HttpGet]
        public ActionResult Register()
        {  
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register model)

        {
            using (HttpClient client = _guestApi.Initial())
            {            
                var content = new StringContent(JsonConvert.SerializeObject(model),
                    Encoding.UTF8,MediaTypeNames.Application.Json);
               
                using (var Response = await client.PostAsync("api/Authentication/Register", content))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["CreateReservation"] = JsonConvert.SerializeObject(model);
                        return RedirectToAction("Index");
                    }
                    else if (Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("CustomError", "Email Already Exist");                
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }

            }
        }

        [HttpGet]
        public ActionResult CreateReservation()
        {
            var model = new ReservationViewModel();
            ViewData["MenuList"] = new List<SelectListItem>{
           new SelectListItem() { Text = "Breakfast", Value = "breakfast" },
           new SelectListItem() { Text = "Lunch", Value = "lunch" },
           new SelectListItem() { Text = "Dinner", Value = "dinner" },
        };
            return View(model);
            
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationViewModel reservation, string returnUrl)
        {
            HttpClient client = _guestApi.Initial();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent( JsonConvert.SerializeObject(reservation), Encoding.UTF8, MediaTypeNames.Application.Json),
                RequestUri = new Uri("https://localhost:44375/api/Reservation/CreateReservation")
            };
           requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var response = await client.SendAsync(requestMessage);
         
            if (response.StatusCode == HttpStatusCode.Unauthorized)

            {
                ModelState.AddModelError("Error", "Unauthorized");
            }
            else

            {
               // ModelState.Clear();
               // ModelState.AddModelError("Sucess", "Successful Reservation Will Contact you Soon");
                return RedirectToAction("Sucess");
            }
          
            return View();          
          
        }
        public  ActionResult Sucess()
        {
            return View();  
        }

        public async Task<IActionResult> Stuff()
        {
            List<ReservationViewModel> reservation = new List<ReservationViewModel>();
            HttpClient client = _guestApi.Initial();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, MediaTypeNames.Application.Json),
                RequestUri = new Uri("https://localhost:44375/api/Reservation/GetReservation")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            var response = await client.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.Unauthorized)

            {
                return Unauthorized();
            }
           
            else

            {
                var result = response.Content.ReadAsStringAsync().Result;
                reservation = JsonConvert.DeserializeObject<List<ReservationViewModel>>(result);
                return View(reservation);
            }
            return NotFound();
        }


      
    } 
}
