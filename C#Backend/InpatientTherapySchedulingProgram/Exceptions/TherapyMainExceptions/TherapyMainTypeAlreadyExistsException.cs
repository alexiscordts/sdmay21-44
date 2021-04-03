using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions
{
    [Serializable]
    public class TherapyMainTypeAlreadyExistsException : Exception
    {
        public TherapyMainTypeAlreadyExistsException()
        { }

        public TherapyMainTypeAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapyMainTypeAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
