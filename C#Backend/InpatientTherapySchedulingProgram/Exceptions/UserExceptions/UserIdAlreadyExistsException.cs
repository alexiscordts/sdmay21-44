using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UserIdAlreadyExistsException : Exception
    {
        public UserIdAlreadyExistsException()
        { }

        public UserIdAlreadyExistsException(string message)
            : base(message)
        { }

        public UserIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
