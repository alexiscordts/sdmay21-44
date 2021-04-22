using System;

namespace InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions
{
    [Serializable]
    public class AppointmentIdAlreadyExistsException : Exception
    {
        public AppointmentIdAlreadyExistsException()
        { }

        public AppointmentIdAlreadyExistsException(string message)
            : base(message)
        { }

        public AppointmentIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
