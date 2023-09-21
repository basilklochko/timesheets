using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Access
{
    public class VendorConsultantRepository : IVendorConsultantRepository
    {
        static OleDbConnection connection;

        protected IUserRepository UserRepository;

        public VendorConsultantRepository(IUserRepository userRepository)
        {
            OleDbHelper.GetConnection(ref connection);

            UserRepository = userRepository;
        }

        public IEnumerable<IModel> GetAllById(int id)
        {
            var result = new List<VendorConsultant>();

            try
            {
                string sql = string.Format("SELECT VendorConsultant.ID AS VendorConsultantId, VendorConsultant.VendorId, VendorConsultant.ConsultantId, VendorConsultant.CreatedDTS AS VendorConsultantCreatedDTS, VendorConsultant.UpdatedDTS AS VendorConsultantUpdatedDTS, [User].* FROM (VendorConsultant INNER JOIN [User] ON VendorConsultant.ConsultantId = [User].id) WHERE (VendorConsultant.VendorId = {0}) ORDER BY 1 DESC", id);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(new User(), sql, OleDbHelper.CommandType.SELECT_DEFINED), connection);
                OleDbHelper.OpenConnection(ref connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new User();

                            var vendorConsultant = new VendorConsultant()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("VendorConsultantId")),
                                VendorId = reader.GetInt32(reader.GetOrdinal("VendorId")),
                                CreatedDTS = reader.GetDateTime(reader.GetOrdinal("VendorConsultantCreatedDTS")),
                                UpdatedDTS = reader.GetDateTime(reader.GetOrdinal("VendorConsultantUpdatedDTS")),
                                Consultant = OleDbHelper.MapReaderRecord(model, model.GetType(), reader) as User
                            };

                            result.Add(vendorConsultant);
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

        public Model.IModel Get(int id)
        {
            IModel model = new VendorConsultant() { id = id };

            try
            {
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.VendorConsultant, OleDbHelper.CommandType.SELECT), connection);
                OleDbHelper.OpenConnection(ref connection);
                model = (VendorConsultant)OleDbHelper.PopulateModel(model, command);
                connection.Close();
            }
            catch
            { 

            }

            return model;
        }

        public int Save(object obj)
        {
            VendorConsultant model = (VendorConsultant)obj;

            try
            {
                model.Consultant.id = UserRepository.Save(model.Consultant);

                OleDbCommand command = null;
                command = new OleDbCommand(string.Format("INSERT INTO [VendorConsultant] ([VendorId], [ConsultantID], [CreatedDTS], [UpdatedDTS]) VALUES({0}, {1}, {2}, {3})", model.VendorId, model.Consultant.id, "#" + DateTime.Now.ToUniversalTime() + "#", "#" + DateTime.Now.ToUniversalTime() + "#"), connection);
                OleDbHelper.OpenConnection(ref connection);
                command.ExecuteNonQuery();
                OleDbHelper.GetIdentity(ref obj, connection);
                connection.Close();

                return model.id;
            }
            catch
            {
                return 0;
            }
        }

        public bool Delete(int id)
        {
            return UserRepository.Delete(id);
        }
    }
}
