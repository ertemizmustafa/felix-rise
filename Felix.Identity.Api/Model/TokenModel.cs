using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Model
{
    public class TokenModel
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }
}
