using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    [Auth]
    public class VendorClientController : ApiController
	{
        protected IVendorClientRepository VendorClientRepository;

        public VendorClientController(IVendorClientRepository vendorClientRepository)
        {
            VendorClientRepository = vendorClientRepository;
        }

		// GET api/vendorclient/5
		public IEnumerable<VendorClient> Get(int id)
		{
            var model = VendorClientRepository.GetAllById(id);

            return model as IEnumerable<VendorClient>;
		}

        // POST api/vendorclient
        public HttpResponseMessage Post(VendorClient vendorClient)
        {
            var result = VendorClientRepository.Save(vendorClient);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // DELETE api/vendorclient/5
        public HttpResponseMessage Delete(int id)
		{
            var result = VendorClientRepository.Delete(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result ? 1.ToString() : 0.ToString())
            };

            return response;
        }
	}
}