using Leav_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Contracts
{
    public interface ILeaveHistoryRepository : IRepositoryBase<LeaveHistory>
    {
        Task<ICollection<LeaveHistory>> GetLeaveHistoriesByEmployee(string employeeId);
        
    }
}
