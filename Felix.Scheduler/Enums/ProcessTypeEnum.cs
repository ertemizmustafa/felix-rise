using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Enums
{
    public enum ProcessTypeEnum : int
    {
        Query = 1,
        Procedure = 2,
        Table = 3,
        RFC = 4,
        HTTPGET = 5,
        HTTPPOST = 6
    }
}
