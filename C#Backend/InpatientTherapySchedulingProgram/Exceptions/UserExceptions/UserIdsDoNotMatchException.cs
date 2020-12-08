using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Exceptions.UserExceptions
{
    [Serializable]
    public class UserIdsDoNotMatchException : Exception
    {
        public UserIdsDoNotMatchException()
        { }

        public UserIdsDoNotMatchException(string message)
            : base(message)
        { }

        public UserIdsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
