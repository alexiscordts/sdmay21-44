using System;

namespace InpatientTherapySchedulingProgram.Exceptions.LocationExceptions
{
    [Serializable]
    public class LocationIdsDoNotMatchException : Exception
    {
        public LocationIdsDoNotMatchException()
        { }

        public LocationIdsDoNotMatchException(string message)
            : base(message)
        { }

        public LocationIdsDoNotMatchException(string message, Exception innerException) 
            : base(message, innerException)
        { }
    }
}
