using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions
{
    [Serializable]
    public class TherapistEventCannotEndBeforeStartTimeException : Exception
    {
        public TherapistEventCannotEndBeforeStartTimeException()
        { }

        public TherapistEventCannotEndBeforeStartTimeException(string message)
            : base(message)
        { }

        public TherapistEventCannotEndBeforeStartTimeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
