using Felix.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Base
{
    public class BaseController : ControllerBase
    {

        public readonly string _logonUserName;

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            _logonUserName = contextAccessor?.HttpContext?.User?.Identity?.Name ?? "";
        }

        protected new IActionResult Ok()
        {
            return base.Ok(EnvelopeHelper.ToEnvelope("İşlem başarılı"));
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(EnvelopeHelper.ToEnvelope(result));
        }

        protected IActionResult Error(string message)
        {
            return base.Ok(EnvelopeHelper.ToEnvelope("", httpStatusCode: Enums.ResponseEnum.InternalError, isSuccessful: false, errorMessage: message));
        }

        protected IActionResult BadRequest(string message)
        {
            return base.Ok(EnvelopeHelper.ToEnvelope("", httpStatusCode: Enums.ResponseEnum.BadRequest, isSuccessful: false, errorMessage: message));
        }

        protected IActionResult UnAuthorized(string message)
        {
            return base.Ok(EnvelopeHelper.ToEnvelope("", httpStatusCode: Enums.ResponseEnum.UnAuthorized, isSuccessful: false, errorMessage: message));
        }

    }
}
