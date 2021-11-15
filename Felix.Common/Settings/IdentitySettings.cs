using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Settings
{
    [Serializable]
    public sealed class IdentitySettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Duration { get; set; }
        public string[] Cors { get; set; }
    }
}
