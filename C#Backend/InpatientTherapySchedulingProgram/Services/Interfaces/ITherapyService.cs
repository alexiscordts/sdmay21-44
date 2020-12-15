using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ITherapyService
    {
        Task<IEnumerable<Therapy>> GetAllTherapies();
        Task<IEnumerable<string>> GetAllAdls();
        Task<IEnumerable<string>> GetAllTypes();
        Task<Therapy> GetTherapyByAdl(string adl);
        Task<Therapy> GetTherapyByAbbreviation(string abbreviation);
        Task<Therapy> UpdateTherapy(string adl, Therapy therapy);
        Task<Therapy> AddTherapy(Therapy therapy);
        Task<Therapy> DeleteTherapy(string adl);

    }
}
