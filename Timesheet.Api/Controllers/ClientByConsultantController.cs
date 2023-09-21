using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    [Auth]
    public class ClientByConsultantController : ApiController
    {
        protected IClientConsultantRepository ClientConsultantRepository;

        public ClientByConsultantController(IClientConsultantRepository clientConsultantRepository)
        {
            ClientConsultantRepository = clientConsultantRepository;
        }

        // GET api/clientbyconsultant/5
        public IEnumerable<ClientConsultant> GetClientsByConsultant(int id)
        {
            var model = ClientConsultantRepository.GetClientsByConsultant(id);

            return model as IEnumerable<ClientConsultant>;
        }
    }
}
