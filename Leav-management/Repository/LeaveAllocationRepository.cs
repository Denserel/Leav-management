using Leav_management.Contracts;
using Leav_management.Data;
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

        public bool Create(LeaveAllocation entity)
        {
            context.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            context.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            return context.LeaveAllocations.ToList();
        }

        public LeaveAllocation FindById(int id)
        {
            return context.LeaveAllocations.Find(id);
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            context.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
