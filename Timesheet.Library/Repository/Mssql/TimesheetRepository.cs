using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Mssql
{
    public class TimesheetRepository: BaseRepository, ITimesheetRepository
    {
        protected IUserRepository UserRepository;

        public TimesheetRepository(IUserRepository userRepository)
            : base()
        {
            UserRepository = userRepository;
        }

        public IEnumerable<Model.IModel> GetAllById(int id, int clientId)
        {
            var all = GetAllById(id) as List<Model.Timesheet>;

            return all.Where(t => t.ClientConsultantId.Equals(clientId));
        }

        public bool ChangeStatus(int id, TimesheetStatus status)
        {
            var result = true;

            var timesheet = (from t in entities.Timesheet
                             where t.id == id
                             select t).FirstOrDefault();

            if (timesheet == null)
            {
                result = false;
            }
            else
            {
                timesheet.Status = (int)status;

                entities.SaveChanges();
            }

            return result;
        }

        public IEnumerable<Model.IModel> GetAllById(int id)
        {
            var result = new List<Model.Timesheet>();
            var user = UserRepository.Get(id) as Model.User;


            switch (user.Type)
            {
                case UserType.Consultant:
                    var consultantData = entities.vwTimesheetConsultant.Where(t => t.ConsultantId == id).ToList();

                    consultantData.ForEach(t =>
                    {
                        var timesheet = new Model.Timesheet()
                        {
                            id = t.id,
                            ClientConsultantId = t.ClientConsultantId,
                            CreatedDTS = t.CreatedDTS,
                            UpdatedDTS = t.UpdatedDTS,
                            Status = (TimesheetStatus)t.Status,
                            TimesheetStatus = ((TimesheetStatus)t.Status).ToString(),
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Worked = t.Worked == null ? 0 : t.Worked.Value,
                            Vendor = new Model.User()
                            {
                                id = t.VendorId,
                                Email = t.VendorEmail,
                                Type = (UserType)t.VendorType,
                                UserName = t.VendorUserName,
                                Address = t.VendorAddress,
                                Phone = t.VendorPhone,
                                Fax = t.VendorFax,
                                Url = t.VendorUrl,
                                TaxCode = t.VendorTaxCode,
                                Logo = t.VendorLogo,
                                Contact = t.VendorContact
                            },
                            Client = new Model.User()
                            {
                                id = t.ClientId,
                                Email = t.ClientEmail,
                                Type = (UserType)t.ClientType,
                                UserName = t.ClientUserName,
                                Address = t.ClientAddress,
                                Phone = t.ClientPhone,
                                Fax = t.ClientFax,
                                Url = t.ClientUrl,
                                TaxCode = t.ClientTaxCode,
                                Logo = t.ClientLogo,
                                Contact = t.ClientContact
                            },
                            Consultant = new Model.User()
                            {
                                id = t.ConsultantId,
                                Email = t.ConsultantEmail,
                                Type = (UserType)t.ConsultantType,
                                UserName = t.ConsultantUserName,
                                Address = t.ConsultantAddress,
                                Phone = t.ConsultantPhone,
                                Fax = t.ConsultantFax,
                                Url = t.ConsultantUrl,
                                TaxCode = t.ConsultantTaxCode,
                                Logo = t.ConsultantLogo,
                                Contact = t.ConsultantContact
                            }
                        };

                        result.Add(timesheet);
                    });
                    break;

                case UserType.Client:
                    var clientData = entities.vwTimesheetConsultant.Where(t => t.ClientId == id).ToList();

                    clientData.ForEach(t =>
                    {
                        var timesheet = new Model.Timesheet()
                        {
                            id = t.id,
                            ClientConsultantId = t.ClientConsultantId,
                            CreatedDTS = t.CreatedDTS,
                            UpdatedDTS = t.UpdatedDTS,
                            Status = (TimesheetStatus)t.Status,
                            TimesheetStatus = ((TimesheetStatus)t.Status).ToString(),
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Worked = t.Worked == null ? 0 : t.Worked.Value,
                            Vendor = new Model.User()
                            {
                                id = t.VendorId,
                                Email = t.VendorEmail,
                                Type = (UserType)t.VendorType,
                                UserName = t.VendorUserName,
                                Address = t.VendorAddress,
                                Phone = t.VendorPhone,
                                Fax = t.VendorFax,
                                Url = t.VendorUrl,
                                TaxCode = t.VendorTaxCode,
                                Logo = t.VendorLogo,
                                Contact = t.VendorContact
                            },
                            Client = new Model.User()
                            {
                                id = t.ClientId,
                                Email = t.ClientEmail,
                                Type = (UserType)t.ClientType,
                                UserName = t.ClientUserName,
                                Address = t.ClientAddress,
                                Phone = t.ClientPhone,
                                Fax = t.ClientFax,
                                Url = t.ClientUrl,
                                TaxCode = t.ClientTaxCode,
                                Logo = t.ClientLogo,
                                Contact = t.ClientContact
                            },
                            Consultant = new Model.User()
                            {
                                id = t.ConsultantId,
                                Email = t.ConsultantEmail,
                                Type = (UserType)t.ConsultantType,
                                UserName = t.ConsultantUserName,
                                Address = t.ConsultantAddress,
                                Phone = t.ConsultantPhone,
                                Fax = t.ConsultantFax,
                                Url = t.ConsultantUrl,
                                TaxCode = t.ConsultantTaxCode,
                                Logo = t.ConsultantLogo,
                                Contact = t.ConsultantContact
                            }
                        };

                        result.Add(timesheet);
                    });
                    break;

                case UserType.Vendor:
                    var vendorData = entities.vwTimesheetVendor.Where(t => t.VendorId == id).ToList();

                    vendorData.ForEach(t =>
                    {
                        var timesheet = new Model.Timesheet()
                        {
                            id = t.id,
                            ClientConsultantId = t.ClientConsultantId,
                            CreatedDTS = t.CreatedDTS,
                            UpdatedDTS = t.UpdatedDTS,
                            Status = (TimesheetStatus)t.Status,
                            TimesheetStatus = ((TimesheetStatus)t.Status).ToString(),
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Worked = t.Worked == null ? 0 : t.Worked.Value,
                            Vendor = new Model.User()
                            {
                                id = t.VendorId,
                                Email = t.VendorEmail,
                                Type = (UserType)t.VendorType,
                                UserName = t.VendorUserName,
                                Address = t.VendorAddress,
                                Phone = t.VendorPhone,
                                Fax = t.VendorFax,
                                Url = t.VendorUrl,
                                TaxCode = t.VendorTaxCode,
                                Logo = t.VendorLogo,
                                Contact = t.VendorContact
                            },
                            Client = new Model.User()
                            {
                                id = t.ClientId,
                                Email = t.ClientEmail,
                                Type = (UserType)t.ClientType,
                                UserName = t.ClientUserName,
                                Address = t.ClientAddress,
                                Phone = t.ClientPhone,
                                Fax = t.ClientFax,
                                Url = t.ClientUrl,
                                TaxCode = t.ClientTaxCode,
                                Logo = t.ClientLogo,
                                Contact = t.ClientContact
                            },
                            Consultant = new Model.User()
                            {
                                id = t.ConsultantId,
                                Email = t.ConsultantEmail,
                                Type = (UserType)t.ConsultantType,
                                UserName = t.ConsultantUserName,
                                Address = t.ConsultantAddress,
                                Phone = t.ConsultantPhone,
                                Fax = t.ConsultantFax,
                                Url = t.ConsultantUrl,
                                TaxCode = t.ConsultantTaxCode,
                                Logo = t.ConsultantLogo,
                                Contact = t.ConsultantContact
                            }
                        };

                        result.Add(timesheet);
                    });
                    break;
            }

            return result;
        }

        public int Save(object obj)
        {
            var model = (Model.Timesheet)obj;

            if (model.Status == TimesheetStatus.Rejected)
            {
                model.Status = TimesheetStatus.Pending;
            }

            model.UpdatedDTS = DateTime.Now.ToUniversalTime();

            Mssql.Timesheet timesheet;

            if (model.id == 0)
            {
                model.CreatedDTS = DateTime.Now.ToUniversalTime();

                timesheet = DbHelper.TimesheetToDb(model, null);

                entities.Timesheet.Add(timesheet);
                entities.SaveChanges();
            }
            else
            {
                entities.TimesheetDay.RemoveRange(entities.TimesheetDay.Where(t => t.TimesheetId == model.id));
                entities.SaveChanges();

                timesheet = entities.Timesheet.FirstOrDefault(t => t.id == model.id);

                if (timesheet != null)
                {
                    timesheet = DbHelper.TimesheetToDb(model, timesheet);

                    entities.SaveChanges();
                }
            }

            model.Days.ForEach(day =>
            {
                var timesheetDay = new Mssql.TimesheetDay()
                {
                    TimesheetId = timesheet.id,
                    Day = day.Day,
                    Worked = day.Worked,
                    CreatedDTS = DateTime.Now.ToUniversalTime(),
                    UpdatedDTS = DateTime.Now.ToUniversalTime(),
                };

                entities.TimesheetDay.Add(timesheetDay);
            });

            entities.SaveChanges();

            return timesheet.id;
        }

        public Model.IModel Get(int id)
        {
            var model = new Model.Timesheet() { id = id };
            model.Days = new List<Model.TimesheetDay>();

            var data = (from timesheet in entities.Timesheet
                        join cc in entities.ClientConsultant on timesheet.ClientConsultantId equals cc.id
                        join vc in entities.VendorClient on cc.VendorClientId equals vc.id
                        join uclient in entities.User on vc.ClientId equals uclient.id
                        join uvendor in entities.User on vc.VendorId equals uvendor.id
                        join vco in entities.VendorConsultant on cc.VendorConsultantId equals vco.id
                        join uconsultant in entities.User on vco.ConsultantId equals uconsultant.id
                        where timesheet.id == id
                        select new
                        {
                            timesheet.ClientConsultantId,
                            ClientId = uclient.id,
                            ClientUserName = uclient.UserName,
                            ClientEmail = uclient.Email,
                            ClientContact = uclient.Contact,
                            VendorUserName = uvendor.UserName,
                            VendorAddress = uvendor.Address,
                            VendorEmail = uvendor.Email,
                            VendorContact = uvendor.Contact,
                            VendorPhone = uvendor.Phone,
                            VendorFax = uvendor.Fax,
                            ConsultantUserName = uconsultant.UserName,
                            ConsultantEmail = uconsultant.Email,
                            timesheet.StartDate,
                            timesheet.EndDate,
                            Status = (TimesheetStatus)timesheet.Status,
                            TimesheetStatus = ((TimesheetStatus)timesheet.Status).ToString(),
                            Comment = timesheet.Comment == null ? string.Empty : timesheet.Comment,
                            timesheet.CreatedDTS,
                            timesheet.UpdatedDTS,
                            Days = entities.TimesheetDay.Where(d => d.TimesheetId == id).ToList()
                        }).FirstOrDefault();

            if (data != null)
            {
                model.ClientConsultantId = data.ClientConsultantId;
                model.Client = new Model.User() { id = data.ClientId, UserName = data.ClientUserName, Email = data.ClientEmail, Contact = data.ClientContact };
                model.Vendor = new Model.User() { UserName = data.VendorUserName, Address = data.VendorAddress, Email = data.VendorEmail, Contact = data.VendorContact, Phone = data.VendorPhone, Fax = data.VendorFax };
                model.Consultant = new Model.User() { UserName = data.ConsultantUserName, Email = data.ConsultantEmail };
                model.StartDate = data.StartDate;
                model.EndDate = data.EndDate;
                model.Status = (TimesheetStatus)data.Status;
                model.TimesheetStatus = ((TimesheetStatus)data.Status).ToString();
                model.Comment = data.Comment == null ? string.Empty : data.Comment;
                model.CreatedDTS = data.CreatedDTS;
                model.UpdatedDTS = data.UpdatedDTS;
            }

            data.Days.ForEach(day =>
            {
                model.Days.Add(new Model.TimesheetDay()
                {
                    Day = day.Day,
                    Worked = day.Worked == null ? 0 : (decimal)day.Worked
                });
            });

            return model;
        }

        public bool Delete(int id)
        {
            IModel model = new Model.Timesheet() { id = id };
            var result = true;

            var timesheet = (from t in entities.Timesheet
                             where t.id == id
                             select t).FirstOrDefault();

            if (timesheet == null)
            {
                result = false;
            }
            else
            {
                entities.Timesheet.Remove(timesheet);
                entities.SaveChanges();
            }

            return result;
        }
    }
}
