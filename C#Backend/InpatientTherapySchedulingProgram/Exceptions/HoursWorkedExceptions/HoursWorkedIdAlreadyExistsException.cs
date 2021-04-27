using System;

namespace InpatientTherapySchedulingProgram.Exceptions.HoursWorkedExceptions
{
    [Serializable]
    public class HoursWorkedIdAlreadyExistsException : Exception
    {
        public HoursWorkedIdAlreadyExistsException()
        { }

        public HoursWorkedIdAlreadyExistsException(string message)
            : base(message)
        { }

        public HoursWorkedIdAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
