using System;

namespace InpatientTherapySchedulingProgram.Exceptions.PatientExceptions
{
    [Serializable]
    public class PatientPidsDoNotMatchException : Exception
    {
        public PatientPidsDoNotMatchException()
        { }

        public PatientPidsDoNotMatchException(string message)
            : base(message)
        { }

        public PatientPidsDoNotMatchException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
