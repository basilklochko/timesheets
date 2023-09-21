using System;
using System.Collections.Generic;
using System.Linq;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Mssql
{
    public class ClientConsultantRepository : BaseRepository, IClientConsultantRepository
    {
        protected IUserRepository UserRepository;

        public ClientConsultantRepository(IUserRepository userRepository)
            : base()
        {
            UserRepository = userRepository;
        }

        public List<Model.ClientConsultant> GetClientsByConsultant(int id)
        {
            var result = new List<Model.ClientConsultant>();
            var data = entities.vwClientByConsultant.Where(v => v.ConsultantId == id).ToList();

            data.ForEach(r =>
            {
                var clientConsultant = new Model.ClientConsultant()
                {
                    id = r.ClientConsultantId,
                    CreatedDTS = r.CreatedDTS,
                    UpdatedDTS = r.UpdatedDTS,
                    Client = new Model.User()
                    {
                        id = r.id,
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
                        Contact = r.Contact
                    }
                };

                result.Add(clientConsultant);
            });


            return result;
        }

        public IEnumerable<IModel> GetAllById(int id)
        {
            var result = new List<Model.ClientConsultant>();
            var data = entities.vwClientConsultant.Where(v => v.ClientVendorId == id && v.ConsultantVendorId == id).ToList();

            data.ForEach(r =>
            {
                var clientConsultant = new Model.ClientConsultant()
                {
                    id = r.ClientConsultantId,
                    CreatedDTS = r.ClientConsultantCreatedDTS,
                    UpdatedDTS = r.ClientConsultantUpdatedDTS,
                    Client = new Model.User()
                    {
                        Email = r.ClientEmail,
                        Password = r.ClientPassword,
                        Type = (UserType)r.ClientType,
                        UserName = r.ClientUserName,
                        Address = r.ClientAddress,
                        Phone = r.ClientPhone,
                        Fax = r.ClientFax,
                        Url = r.ClientUrl,
                        TaxCode = r.ClientTaxCode,
                        Logo = r.ClientLogo,
                        Contact = r.ClientContact
                    },
                    Consultant = new Model.User()
                    {
                        Email = r.ConsultantEmail,
                        Password = r.ConsultantPassword,
                        Type = (UserType)r.ConsultantType,
                        UserName = r.ConsultantUserName,
                        Address = r.ConsultantAddress,
                        Phone = r.ConsultantPhone,
                        Fax = r.ConsultantFax,
                        Url = r.ConsultantUrl,
                        TaxCode = r.ConsultantTaxCode,
                        Logo = r.ConsultantLogo,
                        Contact = r.ConsultantContact
                    }
                };

                result.Add(clientConsultant);
            });

            return result;
        }

        public int Save(object obj)
        {
            var model = (Model.ClientConsultant)obj;
            var clientConsultant = DbHelper.ClientConsultantToDb(model);

            entities.ClientConsultant.Add(clientConsultant);
            entities.SaveChanges();

            return clientConsultant.id;
        }

        public bool Delete(int id)
        {
            var result = (from c in entities.ClientConsultant
                          where c.id == id
                          select c).FirstOrDefault();

            if (result != null)
            {
                entities.ClientConsultant.Remove(result);
                entities.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public IModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
