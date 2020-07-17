using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Front_End.Models
{
    public class GuestApi
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375");
            return client;

        }
        //JWT object
        public string Token { get; set; }

    }
}
