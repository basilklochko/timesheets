using System;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Library.Repository;
using System.Web.Script.Serialization;
using Timesheet.Library.Model;
using Timesheet.Api.Models;

namespace Timesheet.Api.Controllers
{
    public class UserController : ApiController
    {
        protected IUserRepository UserRepository;
        protected IEmailRepository EmailRepository;

        public UserController(IUserRepository userRepository, IEmailRepository emailRepository)
	    {
            UserRepository = userRepository;
            EmailRepository = emailRepository;
	    }

        //// GET api/user/5
        [Auth]
        public HttpResponseMessage Get(int id)
        {
            var model = UserRepository.Get(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(model))
            };

            return response;
        }

        // POST api/user
        public HttpResponseMessage Post(User user)
        {
            var result = 0;
            var newUser = false;

            User model = new User();

            if (user.id == 0)
            {
                newUser = true;

                model = (User)UserRepository.GetByEmail(user.Email);
            }

            if (model.id == 0)
            {
                if (user.id == 0)
                {
                    user.Password = new Random().Next(999999).ToString();
                }

                result = UserRepository.Save(user);

                if (result > 0 && user.Type != UserType.Client)
                {
                    if (newUser)
                    {
                        EmailRepository.Send(user.Email, "Timesheets New Account", string.Format("Congratulations, you account has been created successfully!<br/>Your password is <b>{0}</b>", user.Password));
                    }
                    else
                    {
                        EmailRepository.Send(user.Email, "Timesheets Account Update", string.Format("You account has been updated.<br/>Your password is <b>{0}</b>", user.Password));
                    }
                }
            }

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())                
            };

            return response;
        }
    }
}
