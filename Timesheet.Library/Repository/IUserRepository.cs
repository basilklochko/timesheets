using System;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository
{
	public interface IUserRepository : IRepository
	{
		IModel GetByEmail(string email);

        void StoreToken(Token token);

        Token GetToken(Guid guid);
    }
}
