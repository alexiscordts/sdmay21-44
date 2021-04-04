using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.PatientExceptions
{
    [Serializable]
    public class PatientIdAlreadyExistException : Exception
    {
        public PatientIdAlreadyExistException()
        { }

        public PatientIdAlreadyExistException(string message)
            : base(message)
        { }

        public PatientIdAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

