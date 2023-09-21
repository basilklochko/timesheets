using Timesheet.Library.Model;

namespace Timesheet.Library.Repository
{
	public interface IRepository
	{
		int Save(object obj);
        IModel Get(int id);
		bool Delete(int id);
	}
}
