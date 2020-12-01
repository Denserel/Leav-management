using Leav_management.Contracts;
using Leav_management.Data;
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

        public bool Create(LeaveType entity)
        {
            context.LeaveTypes.Add(entity);
            return Save();
        }

        public bool Delete(LeaveType entity)
        {
            context.LeaveTypes.Remove(entity);
            return Save();
        }

        public ICollection<LeaveType> FindAll()
        {
            return context.LeaveTypes.ToList();
        }

        public LeaveType FindById(int id)
        {
            return context.LeaveTypes.Find(id);
        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public bool isExists(int id)
        {
            return context.LeaveTypes.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }

        public bool Update(LeaveType entity)
        {
            context.LeaveTypes.Update(entity);
            return Save();
        }
    }
}
