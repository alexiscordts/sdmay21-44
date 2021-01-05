using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions
{
    [Serializable]
    public class TherapistActivityDoesNotExistException : Exception
    {
        public TherapistActivityDoesNotExistException()
        { }

        public TherapistActivityDoesNotExistException(string message)
            : base(message)
        { }

        public TherapistActivityDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
