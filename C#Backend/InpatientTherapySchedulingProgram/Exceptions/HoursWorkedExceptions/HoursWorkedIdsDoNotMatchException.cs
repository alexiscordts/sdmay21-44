using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions
{
    [Serializable]
    public class HoursWorkedIdsDoNotMatchException : Exception
    {
        public HoursWorkedIdsDoNotMatchException()
        { }

        public HoursWorkedIdsDoNotMatchException(string message)
            : base(message)
        { }

        public HoursWorkedIdsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
