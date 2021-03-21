using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapistEventExceptions
{
    [Serializable]
    public class TherapistEventEventIdsDoNotMatchException : Exception
    {
        public TherapistEventEventIdsDoNotMatchException()
        { }

        public TherapistEventEventIdsDoNotMatchException(string message)
            : base(message)
        { }

        public TherapistEventEventIdsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
