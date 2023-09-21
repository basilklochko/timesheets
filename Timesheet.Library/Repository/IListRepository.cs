using System.Collections.Generic;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository
{
    public interface IListRepository : IRepository
    {
        IEnumerable<IModel> GetAllById(int id);
    }
}
