using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    [Auth]
    public class VendorConsultantController : ApiController
	{
        protected IVendorConsultantRepository VendorConsultantRepository;
        protected IEmailRepository EmailRepository;

        public VendorConsultantController(IVendorConsultantRepository vendorConsultantRepository, IEmailRepository emailRepository)
        {
            VendorConsultantRepository = vendorConsultantRepository;
            EmailRepository = emailRepository;
        }

        // GET api/vendorconsultant/5
        public IEnumerable<VendorConsultant> Get(int id)
		{
            var model = VendorConsultantRepository.GetAllById(id);

            return model as IEnumerable<VendorConsultant>;
		}

        // POST api/vendorconsultant
        public HttpResponseMessage Post(VendorConsultant vendorConsultant)
        {
            vendorConsultant.Consultant.Password = new Random().Next(999999).ToString();

            var result = VendorConsultantRepository.Save(vendorConsultant);

            if (result > 0)
            {
                EmailRepository.Send(vendorConsultant.Consultant.Email, "Timesheets New Account", string.Format("Congratulations, you account has been created successfully!<br/>Your password is <b>{0}</b>", vendorConsultant.Consultant.Password));
            }

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // DELETE api/vendorconsultant/5
        public HttpResponseMessage Delete(int id)
		{
            var result = VendorConsultantRepository.Delete(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result ? 1.ToString() : 0.ToString())
            };

            return response;
        }
	}
}