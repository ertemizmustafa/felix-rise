using System;

namespace Felix.Schedule.Core
{
    public class BackgroundJobServerSettings
    {
        public string ConnectionString { get; set; }
        public string DbSchemaName { get; set; }
        public string DashboardEnpoint { get; set; }
    }
}
