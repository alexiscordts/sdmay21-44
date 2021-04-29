using System;

namespace InpatientTherapySchedulingProgram.Exceptions.AppointmentExceptions
{
    [Serializable]
    public class AppointmentIdsDoNotMatchException : Exception
    {
        public AppointmentIdsDoNotMatchException()
        { }

        public AppointmentIdsDoNotMatchException(string message)
            : base(message)
        { }

        public AppointmentIdsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
