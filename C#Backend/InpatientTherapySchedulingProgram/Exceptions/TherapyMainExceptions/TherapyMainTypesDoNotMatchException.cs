using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions
{
    [Serializable]
    public class TherapyMainTypesDoNotMatchException : Exception
    {
        public TherapyMainTypesDoNotMatchException()
        { }

        public TherapyMainTypesDoNotMatchException(string message)
            : base(message)
        { }

        public TherapyMainTypesDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
