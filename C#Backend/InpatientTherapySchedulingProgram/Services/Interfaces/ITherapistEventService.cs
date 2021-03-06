using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ITherapistEventService
    {
        Task<IEnumerable<TherapistEvent>> GetTherapistEvents(TherapistEvent therapistEvent);
        Task<IEnumerable<TherapistEvent>> GetTherapistEventsByTherapistId(TherapistEvent therapistEvent);
        Task<TherapistEvent> UpdateTherapistEvent(int eventId, TherapistEvent therapistEvent);
        Task<TherapistEvent> DeleteTherapistEvent(int eventId);
        Task<TherapistEvent> AddTherapistEvent(TherapistEvent therapistEvent);
    }
}
