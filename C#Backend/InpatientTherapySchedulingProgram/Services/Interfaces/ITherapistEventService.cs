using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ITherapistEventService
    {
        Task<IEnumerable<TherapistEvent>> GetAllTherapistEvents(TherapistEvent therapistEvent);
        Task<IEnumerable<TherapistEvent>> GetAllTherapistEventsByTherapistId(TherapistEvent therapistEvent);
        Task<TherapistEvent> UpdateTherapistEvent(int eventId, TherapistEvent therapistEvent);
        Task<TherapistEvent> DeleteTherapistEvent(int eventId);
        Task<TherapistEvent> AddTherapistEvent(TherapistEvent therapistEvent);
    }
}
