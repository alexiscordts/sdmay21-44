using System;

namespace InpatientTherapySchedulingProgram.Exceptions.LocationExceptions
{
    [Serializable]
    public class LocationIdAlreadyExistsException : Exception
    {
        public LocationIdAlreadyExistsException()
        { }

        public LocationIdAlreadyExistsException(string message) 
            : base(message)
        { }

        public LocationIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
