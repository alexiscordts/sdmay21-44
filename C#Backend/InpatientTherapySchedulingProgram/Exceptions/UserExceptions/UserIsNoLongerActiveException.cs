using System;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UserIsNoLongerActiveException : Exception
    {
        public UserIsNoLongerActiveException()
        { }

        public UserIsNoLongerActiveException(string message)
            : base(message)
        { }

        public UserIsNoLongerActiveException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
