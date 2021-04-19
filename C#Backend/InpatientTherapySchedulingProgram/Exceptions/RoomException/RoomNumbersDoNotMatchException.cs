using System;

namespace InpatientTherapySchedulingProgram.Exceptions.RoomException
{
    [Serializable]
    public class RoomNumbersDoNotMatchException : Exception
    {
        public RoomNumbersDoNotMatchException()
        { }

        public RoomNumbersDoNotMatchException(string message)
            : base(message)
        { }

        public RoomNumbersDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
