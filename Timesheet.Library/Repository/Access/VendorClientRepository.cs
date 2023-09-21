using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Access
{
    public class VendorClientRepository : IVendorClientRepository
    {
        static OleDbConnection connection;

        protected IUserRepository UserRepository;

        public VendorClientRepository(IUserRepository userRepository)
        {
            OleDbHelper.GetConnection(ref connection);

            UserRepository = userRepository;
        }

        public IEnumerable<IModel> GetAllById(int id)
        {
            var result = new List<VendorClient>();

            try
            {
                string sql = string.Format("SELECT VendorClient.ID AS VendorClientId, VendorClient.VendorId, VendorClient.ClientId, VendorClient.CreatedDTS AS VendorClientCreatedDTS, VendorClient.UpdatedDTS AS VendorClientUpdatedDTS, [User].* FROM (VendorClient INNER JOIN [User] ON VendorClient.ClientId = [User].id) WHERE (VendorClient.VendorId = {0}) ORDER BY 1 DESC", id);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(new User(), sql, OleDbHelper.CommandType.SELECT_DEFINED), connection);
                OleDbHelper.OpenConnection(ref connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var model = new User();

                            var vendorClient = new VendorClient()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("VendorClientId")),
                                VendorId = reader.GetInt32(reader.GetOrdinal("VendorId")),
                                CreatedDTS = reader.GetDateTime(reader.GetOrdinal("VendorClientCreatedDTS")),
                                UpdatedDTS = reader.GetDateTime(reader.GetOrdinal("VendorClientUpdatedDTS")),
                                Client = OleDbHelper.MapReaderRecord(model, model.GetType(), reader) as User
                            };

                            result.Add(vendorClient);
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
            IModel model = new VendorClient() { id = id };

            try
            {
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.VendorClient, OleDbHelper.CommandType.SELECT), connection);
                OleDbHelper.OpenConnection(ref connection);
                model = (VendorClient)OleDbHelper.PopulateModel(model, command);
                connection.Close();
            }
            catch
            { 

            }

            return model;
        }

        public int Save(object obj)
        {
            VendorClient model = (VendorClient)obj;

            try
            {
                model.Client.id = UserRepository.Save(model.Client);

                OleDbCommand command = null;
                command = new OleDbCommand(string.Format("INSERT INTO [VendorClient] ([VendorId], [ClientID], [CreatedDTS], [UpdatedDTS]) VALUES({0}, {1}, {2}, {3})", model.VendorId, model.Client.id, "#" + DateTime.Now.ToUniversalTime() + "#", "#" + DateTime.Now.ToUniversalTime() + "#"), connection);
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
