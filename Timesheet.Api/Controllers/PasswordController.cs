using System.Net.Http;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    public class PasswordController : ApiController
    {
        private IUserRepository UserRepository;
        private IEmailRepository EmailRepository;
        
        public PasswordController(IUserRepository userRepository, IEmailRepository emailRepository)
	    {
            UserRepository = userRepository;
            EmailRepository = emailRepository;
	    }

        // GET api/password/?email=email
        public HttpResponseMessage Get(string email)
        {
            var result = 0;

            var user = (User)UserRepository.GetByEmail(email);

            if (!string.IsNullOrEmpty(user.Email))
            {
                result = user.id;

                EmailRepository.Send(user.Email, "Timesheets Password Reminder", string.Format("You requested your password to be reminded.<br/>Your password is <b>{0}</b>", user.Password));
            }

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // POST api/password
        [Auth]
        public HttpResponseMessage Post(User data)
        {
            var result = 0;

            var user = (User)UserRepository.Get(data.id);
            user.Password = data.Password;

            result = UserRepository.Save(user);

            if (result > 0)
            {
                EmailRepository.Send(user.Email, "Timesheets Password Change", string.Format("You password has been changed.<br/>Your password is <b>{0}</b>", user.Password));
            }

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }
    }
}
