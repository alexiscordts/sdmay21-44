using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions
{
    [Serializable]
    public class HoursWorkedDoesNotExistException : Exception
    {
        public HoursWorkedDoesNotExistException()
        { }

        public HoursWorkedDoesNotExistException(string message)
            : base(message)
        { }

        public HoursWorkedDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
