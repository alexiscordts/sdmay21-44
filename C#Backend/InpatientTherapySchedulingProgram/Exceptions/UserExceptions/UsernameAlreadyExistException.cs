using System;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UsernameAlreadyExistException : Exception
    {
        public UsernameAlreadyExistException()
        { }

        public UsernameAlreadyExistException(string message)
            : base(message)
        { }

        public UsernameAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
