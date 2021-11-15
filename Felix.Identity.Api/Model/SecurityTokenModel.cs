using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Model
{
    public class SecurityTokenModel
    {
        public string SecurityToken { get; set; }
        public string  UserCode { get; set; }
    }
}
