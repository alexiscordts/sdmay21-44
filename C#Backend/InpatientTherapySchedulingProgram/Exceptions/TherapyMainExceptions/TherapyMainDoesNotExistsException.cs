using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions
{
    [Serializable]
    public class TherapyMainDoesNotExistsException : Exception
    {
        public TherapyMainDoesNotExistsException()
        { }

        public TherapyMainDoesNotExistsException(string message)
            : base(message)
        { }

        public TherapyMainDoesNotExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
