using System;

namespace InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions
{
    [Serializable]
    public class AppointmentCannotEndBeforeStartTimeException : Exception
    {
        public AppointmentCannotEndBeforeStartTimeException()
        { }

        public AppointmentCannotEndBeforeStartTimeException(string message)
            : base(message)
        { }

        public AppointmentCannotEndBeforeStartTimeException(string message, Exception innerException)
            : base(message, innerException)
        { }    
    }
}
