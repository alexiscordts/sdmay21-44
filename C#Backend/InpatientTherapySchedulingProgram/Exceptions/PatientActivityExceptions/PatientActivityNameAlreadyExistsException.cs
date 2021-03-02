using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.PatientActivityExceptions
{
    [Serializable]
    public class PatientActivityNameAlreadyExistsException : Exception
    {
        public PatientActivityNameAlreadyExistsException()
        { }

        public PatientActivityNameAlreadyExistsException(string message)
            : base(message)
        { }

        public PatientActivityNameAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}
