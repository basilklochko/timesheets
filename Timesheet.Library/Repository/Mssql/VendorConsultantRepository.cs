using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Mssql
{
    public class VendorConsultantRepository : BaseRepository, IVendorConsultantRepository
    {
        protected IUserRepository UserRepository;

        public VendorConsultantRepository(IUserRepository userRepository)
            : base()
        {
            UserRepository = userRepository;
        }

        public IEnumerable<Model.IModel> GetAllById(int id)
        {
            var result = new List<Model.VendorConsultant>();
            var data = entities.vwVendorConsultant.Where(v => v.VendorId == id).ToList();

            data.ForEach(r =>
            {
                var vendorConsultant = new Model.VendorConsultant()
                {
                    id = r.VendorConsultantId,
                    VendorId = r.VendorId,
                    CreatedDTS = r.VendorConsultantCreatedDTS,
                    UpdatedDTS = r.VendorConsultantUpdatedDTS,
                    Consultant = new Model.User()
                    {
                        id = r.ConsultantId,
                        Email = r.Email,
                        Password = r.Password,
                        Type = (UserType)r.Type,
                        UserName = r.UserName,
                        Address = r.Address,
                        Phone = r.Phone,
                        Fax = r.Fax,
                        Url = r.Url,
                        TaxCode = r.TaxCode,
                        Logo = r.Logo,
                        Contact = r.Contact,
                        CreatedDTS = r.CreatedDTS,
                        UpdatedDTS = r.UpdatedDTS
                    }
                };

                result.Add(vendorConsultant);
            });

            return result;
        }

        public int Save(object obj)
        {
            var model = (Model.VendorConsultant)obj;

            model.Consultant.id = UserRepository.Save(model.Consultant);

            var vendorConsultant = DbHelper.VendorConsultantToDb(model);
            vendorConsultant.CreatedDTS = DateTime.Now.ToUniversalTime();
            vendorConsultant.UpdatedDTS = DateTime.Now.ToUniversalTime();

            entities.VendorConsultant.Add(vendorConsultant);
            entities.SaveChanges();

            return vendorConsultant.id;
        }

        public Model.IModel Get(int id)
        {
            IModel model = new Model.VendorConsultant() { id = id };

            var result = (from v in entities.VendorConsultant
                          where v.id == id
                          select v).FirstOrDefault();

            if (result != null)
            {
                model = DbHelper.VendorConsultantFromDb(result);
            }

            return model;
        }

        public bool Delete(int id)
        {
            var vendorConsultants = (from v in entities.VendorConsultant
                                 where v.ConsultantId == id
                                 select v).ToList();

            entities.VendorConsultant.RemoveRange(vendorConsultants);
            entities.SaveChanges();

            return UserRepository.Delete(id);
        }
    }
}
