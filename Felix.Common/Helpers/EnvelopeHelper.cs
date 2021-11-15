using Felix.Common.Enums;
using Felix.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Helpers
{
    public static class EnvelopeHelper
    {
        public static Envelope<T> ToEnvelope<T>(this T response, ResponseEnum httpStatusCode = ResponseEnum.Ok, bool isSuccessful = true, string errorMessage = "")
        {
            return new Envelope<T>
            {
                Result = response,
                IsSucessful = isSuccessful,
                Message = errorMessage,
                HttpStatusCode = (int)httpStatusCode
            };
        }
    }
}
