using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ITherapyMainService
    {
        Task<IEnumerable<TherapyMain>> GetAllTherapyMains();
        Task<TherapyMain> GetTherapyMainByType(string type);
        Task<TherapyMain> GetTherapyMainByAbbreviation(string abbreviation);
        Task<TherapyMain> UpdateTherapyMain(string type, TherapyMain therapyMain);
        Task<TherapyMain> AddTherapyMain(TherapyMain therapyMain);
        Task<TherapyMain> DeleteTherapyMain(string type);

    }
}
