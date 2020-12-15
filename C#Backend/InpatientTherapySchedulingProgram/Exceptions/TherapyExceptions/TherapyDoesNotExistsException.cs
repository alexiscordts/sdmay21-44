using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    public class TherapyDoesNotExistsException : Exception
    {
        public TherapyDoesNotExistsException()
        { }

        public TherapyDoesNotExistsException(string message)
            : base(message)
        { }

        public TherapyDoesNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
