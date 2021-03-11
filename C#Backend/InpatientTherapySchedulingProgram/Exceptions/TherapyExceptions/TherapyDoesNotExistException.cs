using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    public class TherapyDoesNotExistException : Exception
    {
        public TherapyDoesNotExistException()
        { }

        public TherapyDoesNotExistException(string message)
            : base(message)
        { }

        public TherapyDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
