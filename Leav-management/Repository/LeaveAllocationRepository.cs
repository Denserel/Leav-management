using Leav_management.Contracts;
using Leav_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext context;

        public LeaveAllocationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            return await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .Where(q => q.EmployeeId == employeeId && q.LeaveTypeId == leaveTypeId && q.Period == period)
                .AnyAsync();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await context.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            context.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            return await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task <ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;

            return await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .Where(q => q.EmployeeId == id && q.Period == period)
                .ToListAsync();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId)
        {
            var period = DateTime.Now.Year;

            return await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.EmployeeId == id && q.Period == period && q.LeaveTypeId == leaveTypeId);
                
        }

        public async Task<bool> isExists(int id)
        {
            return await context.LeaveAllocations.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Save()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            context.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
