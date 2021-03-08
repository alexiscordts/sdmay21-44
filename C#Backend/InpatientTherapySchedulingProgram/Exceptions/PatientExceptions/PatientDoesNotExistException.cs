using System;

namespace InpatientTherapySchedulingProgram.Exceptions.PatientExceptions;

{
    [Serializable]
    public class PatientDoesNotExistException : Exception
    {
        public PatientDoesNotExistException()
        { }

        public PatientDoesNotExistException(string message)
            : base(message)
        { }

        public PatientDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
