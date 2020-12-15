using Leav_management.Contracts;
using Leav_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Repository
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {
        private readonly ApplicationDbContext context;

        public LeaveHistoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Create(LeaveHistory entity)
        {
            await context.LeaveHistories.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveHistory entity)
        {
            context.LeaveHistories.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveHistory>> FindAll()
        {
            return await context.LeaveHistories
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .ToListAsync();
        }

        public async Task<LeaveHistory> FindById(int id)
        {
            return await context.LeaveHistories
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<ICollection<LeaveHistory>> GetLeaveHistoriesByEmployee(string employeeId)
        {
            return await context.LeaveHistories
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .Where(q => q.RequestingEmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> isExists(int id)
        {
            return await context.LeaveHistories.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Save()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(LeaveHistory entity)
        {
            context.LeaveHistories.Update(entity);
            return await Save();
        }
    }
}
