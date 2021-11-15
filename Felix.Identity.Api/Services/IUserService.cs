using Felix.Identity.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Services
{
    public interface IUserService
    {

        public Task<SecurityTokenModel> GetToken(UserInfoModel model);
    }
}
