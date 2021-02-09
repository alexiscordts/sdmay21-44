using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    [Serializable]
    public class TherapyAbbreviationAlreadyExistsException : Exception
    {
        public TherapyAbbreviationAlreadyExistsException()
        { }

        public TherapyAbbreviationAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapyAbbreviationAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
