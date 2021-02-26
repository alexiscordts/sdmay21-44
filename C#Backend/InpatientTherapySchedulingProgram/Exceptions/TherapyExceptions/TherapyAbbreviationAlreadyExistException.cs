using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    [Serializable]
    public class TherapyAbbreviationAlreadyExistException : Exception
    {
        public TherapyAbbreviationAlreadyExistException()
        { }

        public TherapyAbbreviationAlreadyExistException(string message)
            : base(message)
        { }

        public TherapyAbbreviationAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
