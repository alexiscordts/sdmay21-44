using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    [Serializable]
    public class TherapyAdlsDoNotMatchException : Exception
    {
        public TherapyAdlsDoNotMatchException()
        { }

        public TherapyAdlsDoNotMatchException(string message)
            : base(message)
        { }

        public TherapyAdlsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
