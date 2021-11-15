using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Felix.Common.Helpers
{
    public static class ServiceCallHelper
    {

        public static BasicHttpBinding GetBasicHttpBinding(BasicHttpSecurityMode bindingSecurityMode = BasicHttpSecurityMode.None)
        {
            var basicHttpBinding = new BasicHttpBinding(bindingSecurityMode);
            basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            basicHttpBinding.MaxBufferSize = 64000000;
            basicHttpBinding.MaxReceivedMessageSize = 64000000;
            return basicHttpBinding;
        }
    }
}
