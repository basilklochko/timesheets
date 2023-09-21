using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Mssql
{
    public class VendorClientRepository: BaseRepository, IVendorClientRepository
    {
        protected IUserRepository UserRepository;

        public VendorClientRepository(IUserRepository userRepository)
            : base()
        {
            UserRepository = userRepository;
        }

        public IEnumerable<Model.IModel> GetAllById(int id)
        {
            var result = new List<Model.VendorClient>();
            var data = entities.vwVendorClient.Where(v => v.VendorId == id).ToList();

            data.ForEach(r =>
            {
                var vendorClient = new Model.VendorClient()
                {
                    id = r.VendorClientId,
                    VendorId = r.VendorId,
                    CreatedDTS = r.VendorClientCreatedDTS,
                    UpdatedDTS = r.VendorClientUpdatedDTS,
                    Client = new Model.User()
                    {
                        id = r.ClientId,
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

                result.Add(vendorClient);
            });

            return result;
        }

        public int Save(object obj)
        {
            var model = (Model.VendorClient)obj;

            model.Client.id = UserRepository.Save(model.Client);

            var vendorClient = DbHelper.VendorClientToDb(model);
            vendorClient.CreatedDTS = DateTime.Now.ToUniversalTime();
            vendorClient.UpdatedDTS = DateTime.Now.ToUniversalTime();

            entities.VendorClient.Add(vendorClient);
            entities.SaveChanges();

            return vendorClient.id;
        }

        public Model.IModel Get(int id)
        {
            IModel model = new Model.VendorClient() { id = id };

            var result = (from v in entities.VendorClient
                          where v.id == id
                          select v).FirstOrDefault();

            if (result != null)
            {
                model = DbHelper.VendorClientFromDb(result);
            }

            return model;
        }

        public bool Delete(int id)
        {
            var vendorClients = (from v in entities.VendorClient
                                 where v.ClientId == id
                                 select v).ToList();

            entities.VendorClient.RemoveRange(vendorClients);
            entities.SaveChanges();

            return UserRepository.Delete(id);
        }
    }
}
