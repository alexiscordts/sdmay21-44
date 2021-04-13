using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions
{
    [Serializable]
    public class TherapistEventDoesNotExistException : Exception
    {
        public TherapistEventDoesNotExistException()
        { }

        public TherapistEventDoesNotExistException(string message)
            : base(message)
        { }

        public TherapistEventDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
