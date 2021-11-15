using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Felix.Common.Enums
{
    public enum ResponseEnum :int
    {
        [Description("Request Successfull")]
        Ok = 200,
        [Description("No content")]
        NoContent = 204,
        [Description("Unable to process the request")]
        BadRequest = 400,
        [Description("Request Denied")]
        UnAuthorized = 401,
        [Description("Invalid token")]
        AuthenticationFailure = 403,
        [Description("Uri does not exists")]
        NotFound = 404,
        [Description("Request responded with exceptions.")]
        InternalError = 500
    }
}
