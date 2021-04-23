using System;

namespace InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions
{
    [Serializable]
    public class AppointmentDoesNotExistException : Exception
    {
        public AppointmentDoesNotExistException()
        { }

        public AppointmentDoesNotExistException(string message)
            : base(message)
        { }

        public AppointmentDoesNotExistException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
