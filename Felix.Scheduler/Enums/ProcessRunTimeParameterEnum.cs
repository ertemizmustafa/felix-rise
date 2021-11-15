using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Enums
{
    public enum ProcessRunTimeParameterEnum : int
    {
        CurrentDate = 1,
        FirstDayOfMonth = 2,
        LastDayOfMonth = 3,
        CurrentDateValueCombination = 5
    }
}
