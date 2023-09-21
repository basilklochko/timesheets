using System;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
	public class LoginController : ApiController
	{
		private IUserRepository UserRepository;

		public LoginController(IUserRepository userRepository)
		{
			UserRepository = userRepository;
		}

		// GET api/login/5?password=password
		public HttpResponseMessage Get(string email, string password)
		{
			var model = UserRepository.GetByEmail(email);
			var cookie = string.Empty;

			if (model != null)
			{
				var user = model as User;

				if (user.Password == password)
				{
					cookie = string.Format("{0}|{1}|{2}|{3}", user.id, user.UserName, user.Email, user.Type);
				}
			}

			var response = new HttpResponseMessage()
			{
				Content = new StringContent(cookie)
			};
			
			return response;
		}

		//// GET api/user/?token=token
		public HttpResponseMessage Get(string token)
		{
			var cookie = string.Empty;
			var tokenObj = UserRepository.GetToken(Guid.Parse(token));

			if (tokenObj.UserId > 0)
			{
				var user = UserRepository.Get(tokenObj.UserId) as User;

				cookie = string.Format("{0}_{1}_{2}_{3}", user.id, user.UserName, user.Email, user.Type);
			}

			var response = new HttpResponseMessage()
			{
				Content = new StringContent(cookie)
			};

			return response;
		}
	}
}
