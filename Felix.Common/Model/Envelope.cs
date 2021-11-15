using Felix.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Model
{
    [Serializable]
    public class Envelope<T>
    {
        public Envelope()
        {
            RequestId = Guid.NewGuid().ToString();
            ActionTime = DateTime.Now;
            IsSucessful = true;
            HttpStatusCode = (int)ResponseEnum.Ok;

        }

        public string RequestId { get; set; }
        public int HttpStatusCode { get; set; }
        public T Result { get; set; }
        public bool IsSucessful { get; set; }
        public string Message { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
