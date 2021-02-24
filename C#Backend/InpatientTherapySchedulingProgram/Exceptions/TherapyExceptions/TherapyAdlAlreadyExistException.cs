using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    [Serializable]
    public class TherapyAdlAlreadyExistException : Exception
    {
        public TherapyAdlAlreadyExistException()
        { }

        public TherapyAdlAlreadyExistException(string message)
            : base(message)
        { }

        public TherapyAdlAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
