using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions
{
    [Serializable]
    public class TherapistActivityAlreadyExistsException : Exception
    {
        public TherapistActivityAlreadyExistsException()
        { }

        public TherapistActivityAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapistActivityAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
