using System;

namespace InpatientTherapySchedulingProgram.Exceptions.PermissionExceptions
{
    [Serializable]
    public class PermissionAlreadyExistsException : Exception
    {
        public PermissionAlreadyExistsException()
        { }

        public PermissionAlreadyExistsException(string message)
            : base(message)
        { }

        public PermissionAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
