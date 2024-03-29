﻿using Leav_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leav_management.Contracts
{
    public interface ILeaveTypeRepository : IRepositoryBase <LeaveType>
    {
       Task <ICollection<LeaveType>> GetEmployeesByLeaveType(int id);

    }
}
