using Felix.Common.Base;
using Felix.Identity.Api.Model;
using Felix.Identity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : BaseController
    {

        public IUserService _userService;
        public IdentityController(IHttpContextAccessor httpContextAccessor, IUserService userService) : base(httpContextAccessor)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Auth")]
        public async Task<IActionResult> AuthenticateVia(UserInfoModel model)
        {
            return Ok(await _userService.GetToken(model));
        }
    }
}
