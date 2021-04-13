using System;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UserIsNotATherapistException : Exception
    {
        public UserIsNotATherapistException() 
        { }

        public UserIsNotATherapistException(string message)
            : base(message)
        { }

        public UserIsNotATherapistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
