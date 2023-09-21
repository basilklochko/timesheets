using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Access
{
    public class TimesheetRepository : ITimesheetRepository
    {
        static OleDbConnection connection;

        protected IUserRepository UserRepository;

        public TimesheetRepository(IUserRepository userRepository)
        {
            OleDbHelper.GetConnection(ref connection);

            UserRepository = userRepository;
        }

        public IEnumerable<Model.IModel> GetAllById(int id, int clientId)
        { 
            var all = GetAllById(id) as List<Model.Timesheet>;

            return all.Where(t => t.ClientConsultantId.Equals(clientId));
        }

        public IEnumerable<Model.IModel> GetAllById(int id)
        {
            var result = new List<Model.Timesheet>();
            var user = UserRepository.Get(id) as User;

            string sql = string.Empty;

            switch (user.Type)
            { 
                case UserType.Consultant:
                    sql = string.Format("SELECT t.*, (SELECT SUM(Worked) FROM [TimesheetDay] d WHERE t.id = d.TimesheetId) AS Worked, uvendor.*, uclient.*, uconsultant.* FROM ((((([Timesheet] t INNER JOIN [ClientConsultant] cc ON t.ClientConsultantId = cc.id) INNER JOIN [VendorClient] vclient ON (vclient.Id = cc.VendorClientId)) INNER JOIN [User] uclient ON uclient.id = vclient.ClientId) INNER JOIN [VendorConsultant] vconsultant ON (vconsultant.Id = cc.VendorConsultantId AND vconsultant.ConsultantId = {0})) INNER JOIN [User] uconsultant ON uconsultant.id = vconsultant.ConsultantId) INNER JOIN [User] uvendor ON uvendor.id = vconsultant.VendorId ORDER BY ClientConsultantId, t.EndDate DESC", user.id);
                    break;

                case UserType.Client:
                    sql = string.Format("SELECT t.*, (SELECT SUM(Worked) FROM [TimesheetDay] d WHERE t.id = d.TimesheetId) AS Worked, uvendor.*, uclient.*, uconsultant.* FROM ((((([Timesheet] t INNER JOIN [ClientConsultant] cc ON t.ClientConsultantId = cc.id) INNER JOIN [VendorClient] vclient ON (vclient.Id = cc.VendorClientId)) INNER JOIN [User] uclient ON (uclient.id = vclient.ClientId AND uclient.id = {0})) INNER JOIN [VendorConsultant] vconsultant ON vconsultant.Id = cc.VendorConsultantId) INNER JOIN [User] uconsultant ON uconsultant.id = vconsultant.ConsultantId) INNER JOIN [User] uvendor ON uvendor.id = vconsultant.VendorId ORDER BY ClientConsultantId, t.EndDate DESC", user.id);
                    break;

                case UserType.Vendor:
                    sql = string.Format("SELECT t.*, (SELECT SUM(Worked) FROM [TimesheetDay] d WHERE t.id = d.TimesheetId) AS Worked, uvendor.*, uclient.*, uconsultant.* FROM ((((([Timesheet] t INNER JOIN [ClientConsultant] cc ON t.ClientConsultantId = cc.id) INNER JOIN [VendorClient] vclient ON (vclient.Id = cc.VendorClientId)) INNER JOIN [User] uclient ON uclient.id = vclient.ClientId) INNER JOIN [VendorConsultant] vconsultant ON vconsultant.Id = cc.VendorConsultantId) INNER JOIN [User] uconsultant ON uconsultant.id = vconsultant.ConsultantId) INNER JOIN [User] uvendor ON (uvendor.id = vconsultant.VendorId AND uvendor.id = {0}) ORDER BY ClientConsultantId, t.EndDate DESC", user.id);
                    break;
            }

            try
            {
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(new User(), sql, OleDbHelper.CommandType.SELECT_DEFINED), connection);
                OleDbHelper.OpenConnection(ref connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var vendor = new User();
                            var client = new User();
                            var consultant = new User();

                            var timesheet = new Model.Timesheet()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("t.Id")),
                                ClientConsultantId = reader.GetInt32(reader.GetOrdinal("ClientConsultantID")),
                                CreatedDTS = reader.GetDateTime(reader.GetOrdinal("t.CreatedDTS")),
                                UpdatedDTS = reader.GetDateTime(reader.GetOrdinal("t.UpdatedDTS")),
                                Status = (TimesheetStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                TimesheetStatus = ((TimesheetStatus)reader.GetInt32(reader.GetOrdinal("Status"))).ToString(),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                Worked = reader.GetValue(reader.GetOrdinal("Worked")) == DBNull.Value ? 0 : reader.GetDecimal(reader.GetOrdinal("Worked")),
                                Vendor = OleDbHelper.MapReaderRecord(vendor, vendor.GetType(), reader, "uvendor.") as User,
                                Client = OleDbHelper.MapReaderRecord(client, client.GetType(), reader, "uclient.") as User,
                                Consultant = OleDbHelper.MapReaderRecord(consultant, consultant.GetType(), reader, "uconsultant.") as User
                            };

                            result.Add(timesheet);
                        }
                    }
                }
                connection.Close();
            }
            catch 
            {

            }

            return result;
        }

        public int Save(object obj)
        {
            Model.Timesheet model = (Model.Timesheet)obj;

            if (model.Status == TimesheetStatus.Rejected)
            {
                model.Status = TimesheetStatus.Pending;
            }

            try
            {
                var tsql = string.Empty;
                var dsql = "INSERT INTO [TimesheetDay] ([TimesheetId], [Day], [Worked], [CreatedDTS], [UpdatedDTS]) VALUES({0}, #{1}#, {2}, #{3}#, #{4}#)";
                OleDbCommand command = null;
                OleDbHelper.OpenConnection(ref connection);
                if (model.id == 0)
                {                    
                    tsql = string.Format("INSERT INTO [Timesheet] ([ClientConsultantId], [Status], [StartDate], [EndDate], [CreatedDTS], [UpdatedDTS], [Comment]) VALUES({0}, {1}, #{2}#, #{3}#, #{4}#, #{5}#, '{6}')",
                        model.ClientConsultantId, (int)model.Status, model.StartDate.ToUniversalTime(), model.EndDate.ToUniversalTime(), DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime(), model.Comment);
                    command = new OleDbCommand(tsql, connection);
                }
                else
                {
                    command = new OleDbCommand(string.Format("DELETE FROM [TimesheetDay] WHERE TimesheetId = {0}", model.id), connection);
                    command.ExecuteNonQuery();
                    tsql = string.Format("UPDATE [Timesheet] SET [StartDate] = #{0}#, [EndDate] = #{1}#, [UpdatedDTS] = #{2}#, [Comment] = '{3}', [Status] = {5} WHERE id = {4}",
                        model.StartDate.ToUniversalTime(), model.EndDate.ToUniversalTime(), DateTime.Now.ToUniversalTime(), model.Comment, model.id, (int)model.Status);
                    command = new OleDbCommand(tsql, connection);
                }
                command.ExecuteNonQuery();
                OleDbHelper.GetIdentity(ref obj, connection);
                if (model.Days != null)
                {
                    foreach (var day in model.Days)
                    {
                        var sql = string.Format(dsql, model.id, day.Day.ToUniversalTime(), day.Worked, DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime());
                        command = new OleDbCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();

                return model.id;
            }
            catch
            {
                return 0;
            }
        }

        public Model.IModel Get(int id)
        {
            var model = new Model.Timesheet() { id = id };
            model.Days = new List<TimesheetDay>();

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                string sql = string.Format("SELECT uclient.id AS ClientId, uclient.UserName, uclient.Email, uclient.Contact, uvendor.UserName, uvendor.Address, uvendor.Email, uvendor.Contact, uvendor.Phone, uvendor.Fax, uconsultant.UserName, uconsultant.Email, timesheet.*, days.* FROM ((((((([Timesheet] timesheet INNER JOIN [ClientConsultant]cc ON cc.id = timesheet.ClientConsultantId) INNER JOIN [VendorClient] vc ON vc.id = cc.VendorClientId) INNER JOIN [User] uclient ON uclient.id = vc.ClientId) INNER JOIN [User] uvendor ON uvendor.id = vc.VendorId) INNER JOIN [VendorConsultant] vco ON vco.id = cc.VendorConsultantId) INNER JOIN [User] uconsultant ON uconsultant.id = vco.ConsultantId) LEFT JOIN [TimesheetDay] days ON days.TimesheetId = timesheet.id) WHERE timesheet.id = {0}", id); 
                OleDbCommand command = new OleDbCommand(sql, connection);                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var i = 0;
                        while (reader.Read())
                        {
                            if (i == 0)
                            {
                                model.ClientConsultantId = reader.GetInt32(reader.GetOrdinal("ClientConsultantID"));
                                model.Client = new User() { id = reader.GetInt32(reader.GetOrdinal("ClientId")), UserName = reader.GetString(reader.GetOrdinal("uclient.UserName")), Email = reader.GetString(reader.GetOrdinal("uclient.Email")), Contact = reader.GetString(reader.GetOrdinal("uclient.Contact")) };
                                model.Vendor = new User() { UserName = reader.GetString(reader.GetOrdinal("uvendor.UserName")), Address = reader.GetString(reader.GetOrdinal("Address")), Email = reader.GetString(reader.GetOrdinal("uvendor.Email")), Contact = reader.GetString(reader.GetOrdinal("uvendor.Contact")), Phone = reader.GetString(reader.GetOrdinal("Phone")), Fax = reader.GetString(reader.GetOrdinal("Fax")) };
                                model.Consultant = new User() { UserName = reader.GetString(reader.GetOrdinal("uconsultant.UserName")), Email = reader.GetString(reader.GetOrdinal("uconsultant.Email")) };
                                model.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
                                model.EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"));
                                model.Status = (TimesheetStatus)reader.GetInt32(reader.GetOrdinal("Status"));
                                model.TimesheetStatus = ((TimesheetStatus)reader.GetInt32(reader.GetOrdinal("Status"))).ToString();
                                model.Comment = reader.GetValue(reader.GetOrdinal("Comment")) == DBNull.Value ? string.Empty : reader.GetString(reader.GetOrdinal("Comment"));
                                model.CreatedDTS = reader.GetDateTime(reader.GetOrdinal("timesheet.CreatedDTS"));
                                model.UpdatedDTS = reader.GetDateTime(reader.GetOrdinal("timesheet.UpdatedDTS"));
                            }

                            model.Days.Add(new TimesheetDay()
                            {
                                Day = reader.GetDateTime(reader.GetOrdinal("Day")),
                                Worked = reader.GetValue(reader.GetOrdinal("Worked")) == DBNull.Value ? 0 : reader.GetDecimal(reader.GetOrdinal("Worked"))
                            });

                            i++;
                        }
                    }
                }
                
                connection.Close();
            }
            catch
            {

            }

            return model;
        }

        public bool Delete(int id)
        {
            IModel model = new Model.Timesheet() { id = id };
            var result = true;

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.Timesheet, OleDbHelper.CommandType.DELETE), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                result = false;
            }

            return result;
        }


        public bool ChangeStatus(int id, TimesheetStatus status)
        {
            var result = true;

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(String.Format("UPDATE [Timesheet] SET [Status] = {0}, [UpdatedDTS] = #{1}# WHERE id = {2}", (int)status, DateTime.Now.ToUniversalTime(), id), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                result = false;
            }

            return result;            
        }
    }
}
