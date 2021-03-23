using InpatientTherapySchedulingProgram.Models;
using InpatientTherapySchedulingProgram.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InpatientTherapySchedulingProgram.Services
{
    public class RoomService : IRoomService
    {
        private readonly CoreDbContext _context;

    }
}
