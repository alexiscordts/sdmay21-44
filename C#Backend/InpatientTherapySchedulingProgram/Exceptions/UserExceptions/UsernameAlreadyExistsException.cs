using System;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException()
        { }

        public UsernameAlreadyExistsException(string message)
            : base(message)
        { }

        public UsernameAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
