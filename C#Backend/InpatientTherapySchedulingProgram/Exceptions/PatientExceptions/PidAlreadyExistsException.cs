using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.PatientExceptions
{
    [Serializable]
    public class PidAlreadyExistsException : Exception
    {
        public PidAlreadyExistsException()
        { }

        public PidAlreadyExistsException(string message)
            : base(message)
        { }

        public PidAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

