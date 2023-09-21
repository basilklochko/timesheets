using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    [Auth]
    public class ClientConsultantController : ApiController
	{
        protected IClientConsultantRepository ClientConsultantRepository;

        public ClientConsultantController(IClientConsultantRepository clientConsultantRepository)
        {
            ClientConsultantRepository = clientConsultantRepository;
        }

		// GET api/clientconsultant/5
		public IEnumerable<ClientConsultant> Get(int id)
		{
            var model = ClientConsultantRepository.GetAllById(id);

            return model as IEnumerable<ClientConsultant>;
		}

        // POST api/vendorclient
        public HttpResponseMessage Post(ClientConsultant clientConsultant)
        {
            var result = ClientConsultantRepository.Save(clientConsultant);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // DELETE api/vendorclient/5
        public HttpResponseMessage Delete(int id)
		{
            var result = ClientConsultantRepository.Delete(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result ? 1.ToString() : 0.ToString())
            };

            return response;
        }
	}
}