using System;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UserIdAlreadyExistException : Exception
    {
        public UserIdAlreadyExistException()
        { }

        public UserIdAlreadyExistException(string message)
            : base(message)
        { }

        public UserIdAlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
