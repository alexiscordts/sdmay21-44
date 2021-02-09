using System;

namespace InpatientTherapySchedulingProgram.Exceptions.TherapyExceptions
{
    [Serializable]
    public class TherapyAdlAlreadyExistsException : Exception
    {
        public TherapyAdlAlreadyExistsException()
        { }

        public TherapyAdlAlreadyExistsException(string message)
            : base(message)
        { }

        public TherapyAdlAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
