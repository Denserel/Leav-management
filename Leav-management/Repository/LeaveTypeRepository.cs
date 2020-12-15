using Leav_management.Contracts;
using Leav_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext context;

        public LeaveTypeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(LeaveType entity)
        {
            await context.LeaveTypes.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            context.LeaveTypes.Remove(entity);
            return await Save();
        }
        public async Task<bool> Update(LeaveType entity)
        {
            context.LeaveTypes.Update(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            return await context.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType> FindById(int id)
        {
            return await context.LeaveTypes.FindAsync(id);
        }

        public async Task<ICollection<LeaveType>> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            return await context.LeaveTypes.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Save()
        {
            return await context.SaveChangesAsync() > 0;
        }

        
    }
}
