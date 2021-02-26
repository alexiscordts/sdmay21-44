using System;

namespace InpatientTherapySchedulingProgram.Exceptions.LocationExceptions
{
    [Serializable]
    public class LocationNameAlreadyExistsException : Exception
    {
        public LocationNameAlreadyExistsException()
        { }

        public LocationNameAlreadyExistsException(string message)
            : base(message)
        { }

        public LocationNameAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
