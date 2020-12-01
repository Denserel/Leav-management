using Leav_management.Contracts;
using Leav_management.Data;
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
        public bool Create(LeaveHistory entity)
        {
            context.LeaveHistories.Add(entity);
            return Save();
        }

        public bool Delete(LeaveHistory entity)
        {
            context.LeaveHistories.Remove(entity);
            return Save();
        }

        public ICollection<LeaveHistory> FindAll()
        {
            return context.LeaveHistories.ToList();
        }

        public LeaveHistory FindById(int id)
        {
            return context.LeaveHistories.Find(id);
        }

        public bool isExists(int id)
        {
            return context.LeaveHistories.Any(x => x.Id == id);
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }

        public bool Update(LeaveHistory entity)
        {
            context.LeaveHistories.Update(entity);
            return Save();
        }
    }
}
