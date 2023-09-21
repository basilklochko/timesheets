
using System;
using System.Collections.Generic;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository
{
    public interface ITimesheetRepository : IListRepository
	{
        IEnumerable<Model.IModel> GetAllById(int id, int clientId);

        bool ChangeStatus(int id, Model.TimesheetStatus status);
	}
}
