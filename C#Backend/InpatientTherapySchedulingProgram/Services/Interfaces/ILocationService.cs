using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocations();
        Task<IEnumerable<string>> GetAllLocationNames();
        Task<Location> GetLocationByLocationId(int id);
        Task<Location> GetLocationByName(string name);
        Task<Location> UpdateLocation(int id, Location location);
        Task<Location> AddLocation(Location location);
        Task<Location> DeleteLocation(int id);
    }
}
