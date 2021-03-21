using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions
{
    [Serializable]
    public class TherapistEventEventIdAlreadyExistsException : Exception
    {
        public TherapistEventEventIdAlreadyExistsException()
        { }

        public TherapistEventEventIdAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapistEventEventIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
