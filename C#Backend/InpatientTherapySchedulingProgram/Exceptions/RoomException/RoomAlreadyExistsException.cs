using System;

namespace InpatientTherapySchedulingProgram.Exceptions.RoomException
{
    [Serializable]
    public class RoomAlreadyExistsException : Exception
    {
        public RoomAlreadyExistsException()
        { }

        public RoomAlreadyExistsException(string message)
            : base(message)
        { }

        public RoomAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
