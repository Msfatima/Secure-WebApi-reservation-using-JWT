using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        //private readonly IEmailSender _emailSender; UnComment if you want to add Email Verification also.

        public AuthenticationController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)

                }),
                    Issuer = "https://localhost:44375",
                    Audience = "https://localhost:44375",
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(new 
                    SymmetricSecurityKey(Encoding.UTF8.GetBytes("CreateAnewReservationneed2toLOGINE")), 
                    SecurityAlgorithms.HmacSha512Signature),
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    expiration = token.ValidTo 
                });
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Register Input)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    //DO YOUR STUFF HERE

                    //EMAIL VERIFICATION CODE
                    //If you want to add Email verificiation then uncoment two lines of Email and add Scoped services in Startup.cs with Definations.
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code },
                    //    protocol: Request.Scheme);                 
                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    return Ok("Successful Registration"); //Send Result or Message whatever you like.
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return new JsonResult(ModelState);
                }

            }
            else
            {
                return new JsonResult("Error Here"); ;
            }
        }

    }
}
