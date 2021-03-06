using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions
{
    [Serializable]
    public class EventIdAlreadyExistsException : Exception
    {
        public EventIdAlreadyExistsException()
        { }

        public EventIdAlreadyExistsException(string message)
            : base(message)
        { }

        public EventIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
