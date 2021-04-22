using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions
{
    [Serializable]
    public class TherapyMainDoesNotExistException : Exception
    {
        public TherapyMainDoesNotExistException()
        { }

        public TherapyMainDoesNotExistException(string message)
            : base(message)
        { }

        public TherapyMainDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
