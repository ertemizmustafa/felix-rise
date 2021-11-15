using Felix.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Services
{
    public interface ITokenService
    {

        Task<SecurityTokenModel> GeneterateJwtToken(TokenModel tokenModel);
    }
}
