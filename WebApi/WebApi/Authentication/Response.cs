﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authentication
{
    public class Response
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
