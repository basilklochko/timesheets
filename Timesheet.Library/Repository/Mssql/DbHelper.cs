using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheet.Library.Repository.Mssql
{
    public static class DbHelper
    {
        public static Mssql.Timesheet TimesheetToDb(Model.Timesheet timesheet, Mssql.Timesheet existingTimesheet)
        {
            if (timesheet == null)
            {
                return null;
            }

            if (existingTimesheet == null)
            {
                existingTimesheet = new Mssql.Timesheet();
            }

            existingTimesheet.CreatedDTS = timesheet.CreatedDTS;
            existingTimesheet.UpdatedDTS = timesheet.UpdatedDTS;
            existingTimesheet.ClientConsultantId = timesheet.ClientConsultantId;
            existingTimesheet.Status = (int)timesheet.Status;
            existingTimesheet.StartDate = timesheet.StartDate;
            existingTimesheet.EndDate = timesheet.EndDate;
            existingTimesheet.Comment = timesheet.Comment;

            return existingTimesheet;
        }


        public static Mssql.VendorConsultant VendorConsultantToDb(Model.VendorConsultant vendorConsultant)
        {
            if (vendorConsultant == null)
            {
                return null;
            }

            var result = new Mssql.VendorConsultant()
            {
                CreatedDTS = vendorConsultant.CreatedDTS,
                UpdatedDTS = vendorConsultant.UpdatedDTS,
                VendorId = vendorConsultant.VendorId,
                ConsultantId = vendorConsultant.Consultant.id
            };

            return result;
        }

        public static Model.VendorConsultant VendorConsultantFromDb(Mssql.VendorConsultant vendorConsultant)
        {
            if (vendorConsultant == null)
            {
                return null;
            }

            var result = new Model.VendorConsultant()
            {
                id = vendorConsultant.id,
                CreatedDTS = vendorConsultant.CreatedDTS,
                UpdatedDTS = vendorConsultant.UpdatedDTS,
                VendorId = vendorConsultant.VendorId,
                Consultant = new Model.User()
                {
                    id = vendorConsultant.ConsultantId,
                }
            };

            return result;

        }
        public static Model.VendorClient VendorClientFromDb(Mssql.VendorClient vendorClient)
        {
            if (vendorClient == null)
            {
                return null;
            }

            var result = new Model.VendorClient()
            {
                id = vendorClient.id,
                CreatedDTS = vendorClient.CreatedDTS,
                UpdatedDTS = vendorClient.UpdatedDTS,
                VendorId = vendorClient.VendorId,
                Client = new Model.User()
                {
                    id = vendorClient.ClientId,
                }
            };

            return result;
        }

        public static Mssql.VendorClient VendorClientToDb(Model.VendorClient vendorClient)
        {
            if (vendorClient == null)
            {
                return null;
            }

            var result = new Mssql.VendorClient()
            {
                CreatedDTS = vendorClient.CreatedDTS,
                UpdatedDTS = vendorClient.UpdatedDTS,
                VendorId = vendorClient.VendorId,
                ClientId = vendorClient.Client.id
            };

            return result;
        }

        public static Mssql.ClientConsultant ClientConsultantToDb(Model.ClientConsultant clientConsultant)
        {
            if (clientConsultant == null)
            {
                return null;
            }

            var result = new Mssql.ClientConsultant()
            {
                CreatedDTS = clientConsultant.CreatedDTS,
                UpdatedDTS = clientConsultant.UpdatedDTS,
                VendorClientId = clientConsultant.Client.id,
                VendorConsultantId = clientConsultant.Consultant.id
            };

            return result;
        }

        public static Model.Token TokenFromDb(Mssql.SecurityToken token)
        {
            if (token == null)
            {
                return null;
            }

            var result = new Model.Token()
            {
                id = token.id,
                Guid = Guid.Parse(token.Token),
                UserId = token.UserId
            };

            return result;
        }

        public static Mssql.SecurityToken TokenToDb(Model.Token token)
        {
            if (token == null)
            {
                return null;
            }

            var securityToken = new SecurityToken()
            {
                Token = token.Guid.ToString(),
                UserId = token.UserId
            };

            return securityToken;
        }

        public static Model.User UserFromDb(Mssql.User user)
        {
            if (user == null)
            {
                return null;
            }

            var result = new Model.User()
            {
                Address = user.Address,
                Contact = user.Contact,
                CreatedDTS = user.CreatedDTS,
                Email = user.Email,
                Fax = user.Fax,
                id = user.id,
                Logo = user.Logo,
                Password = user.Password,
                Phone = user.Phone,
                TaxCode = user.TaxCode,
                Type = (Model.UserType)user.Type,
                UpdatedDTS = user.UpdatedDTS,
                Url = user.Url,
                UserName = user.UserName
            };

            return result;
        }

        public static Mssql.User UserToDb(Model.User user, Mssql.User existingUser)
        {
            if (user == null)
            {
                return null;
            }

            if (existingUser == null)
            {
                existingUser = new Mssql.User(); 
            }

            existingUser.Address = user.Address;
            existingUser.Contact = user.Contact;
            existingUser.CreatedDTS = user.CreatedDTS;
            existingUser.Email = user.Email;
            existingUser.Fax = user.Fax;
            existingUser.id = user.id;
            existingUser.Logo = user.Logo;
            existingUser.Password = user.Password;
            existingUser.Phone = user.Phone;
            existingUser.TaxCode = user.TaxCode;
            existingUser.Type = (int)user.Type;
            existingUser.UpdatedDTS = user.UpdatedDTS;
            existingUser.Url = user.Url;
            existingUser.UserName = user.UserName;

            return existingUser;
        }
    }
}
