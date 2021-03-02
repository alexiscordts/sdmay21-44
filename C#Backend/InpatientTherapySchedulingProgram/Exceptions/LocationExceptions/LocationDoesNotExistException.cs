using System;

namespace InpatientTherapySchedulingProgram.Exceptions.LocationExceptions
{
    [Serializable]
    public class LocationDoesNotExistException : Exception
    {
        public LocationDoesNotExistException()
        { }

        public LocationDoesNotExistException(string message)
            : base(message)
        { }

        public LocationDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
