using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyMainExceptions
{
    [Serializable]
    public class TherapyMainTypeAbbreviationAlreadyExistsException : Exception
    {
        public TherapyMainTypeAbbreviationAlreadyExistsException()
        { }

        public TherapyMainTypeAbbreviationAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapyMainTypeAbbreviationAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
