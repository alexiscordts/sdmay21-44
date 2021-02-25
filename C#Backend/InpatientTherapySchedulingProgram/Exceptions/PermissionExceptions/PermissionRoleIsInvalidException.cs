using System;

namespace InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions
{
    [Serializable]
    public class PermissionRoleIsInvalidException : Exception
    {
        public PermissionRoleIsInvalidException()
        { }

        public PermissionRoleIsInvalidException(string message)
            : base(message)
        { }

        public PermissionRoleIsInvalidException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
