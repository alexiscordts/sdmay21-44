using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IPatientActivity
    {
        Task<IEnumerable<PatientActivity>> GetAllPatientActivity();
        Task<PatientActivity> GetPatientActivityByName(string activityName);
        Task<PatientActivity> AddPatientActivity(PatientActivity patientActivity);
        Task<PatientActivity> DeletePatientActivity(string activityName);
    }
}
}
