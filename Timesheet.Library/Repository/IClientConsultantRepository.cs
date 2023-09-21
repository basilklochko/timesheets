using System.Collections.Generic;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository
{
    public interface IClientConsultantRepository : IListRepository
	{
        List<ClientConsultant> GetClientsByConsultant(int id);
	}
}
