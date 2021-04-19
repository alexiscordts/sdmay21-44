using System;

namespace InpatientTherapySchedulingProgram.Exceptions.RoomException
{
    [Serializable]
    public class RoomDoesNotExistException : Exception
    {
        public RoomDoesNotExistException()
        { }

        public RoomDoesNotExistException(string message)
            : base(message)
        { }

        public RoomDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
