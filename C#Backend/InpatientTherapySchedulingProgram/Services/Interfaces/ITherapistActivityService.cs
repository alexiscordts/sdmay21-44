using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ITherapistActivityService
    {
        Task<IEnumerable<TherapistActivity>> GetAllTherapistActivities();
        Task<TherapistActivity> GetTherapistActivityByName(string name);
        Task<TherapistActivity> UpdateTherapistActivity(string name, TherapistActivity therapistActivity);
        Task<TherapistActivity> AddTherapistActivity(TherapistActivity therapistActivity);
        Task<TherapistActivity> DeleteTherapistActivity(string name);
    }
}
