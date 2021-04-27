using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface IHoursWorkedService
    {
        Task<IEnumerable<HoursWorked>> GetHoursWorkedByUserId(int userId);
        Task<HoursWorked> GetHoursWorkedById(int hoursWorkedId);
        Task<HoursWorked> UpdateHoursWorked(int hoursWorkedId, HoursWorked hoursWorked);
        Task<HoursWorked> AddHoursWorked(HoursWorked hoursWorked);
        Task<HoursWorked> DeleteHoursWorked(int hoursWorkedId);
    }
}
