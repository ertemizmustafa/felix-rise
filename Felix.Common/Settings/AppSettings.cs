using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Settings
{
    [Serializable]
    public sealed class AppSettings
    {
        public string BaseUri { get; set; }
        public string[] DomainNames { get; set; }
    }
}
