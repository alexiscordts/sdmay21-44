using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistActivityExceptions
{
    [Serializable]
    public class TherapistActivityNamesDoNotMatchException : Exception
    {
        public TherapistActivityNamesDoNotMatchException()
        { }

        public TherapistActivityNamesDoNotMatchException(string message)
            : base(message)
        { }

        public TherapistActivityNamesDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
