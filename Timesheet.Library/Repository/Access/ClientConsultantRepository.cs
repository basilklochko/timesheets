using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Access
{
    public class ClientConsultantRepository : IClientConsultantRepository
    {
        static OleDbConnection connection;

        protected IUserRepository UserRepository;

        public ClientConsultantRepository(IUserRepository userRepository)
        {
            OleDbHelper.GetConnection(ref connection);

            UserRepository = userRepository;
        }

        public IEnumerable<IModel> GetAllById(int id)
        {
            var result = new List<ClientConsultant>();

            try
            {
                string sql = string.Format("SELECT cc.id AS ClientConsultantId, cc.CreatedDTS AS ClientConsultantCreatedDTS, cc.UpdatedDTS AS ClientConsultantUpdatedDTS, uclient.*, uconsultant.* FROM ((([ClientConsultant] cc INNER JOIN [VendorClient] client ON client.id = cc.VendorClientId) INNER JOIN [VendorConsultant] consultant ON consultant.id = cc.VendorConsultantId) INNER JOIN [User] uclient ON uclient.id = client.ClientId) INNER JOIN [User] uconsultant ON uconsultant.id = consultant.ConsultantId WHERE (client.VendorId = {0} AND consultant.VendorId = {0}) ORDER BY 2 DESC, uclient.UserName, uclient.Contact", id); 
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(new User(), sql, OleDbHelper.CommandType.SELECT_DEFINED), connection);
                OleDbHelper.OpenConnection(ref connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var client = new User();
                            var consultant = new User();

                            var clientConsultant = new ClientConsultant()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("ClientConsultantId")),
                                CreatedDTS = reader.GetDateTime(reader.GetOrdinal("ClientConsultantCreatedDTS")),
                                UpdatedDTS = reader.GetDateTime(reader.GetOrdinal("ClientConsultantUpdatedDTS")),
                                Client = OleDbHelper.MapReaderRecord(client, client.GetType(), reader, "uclient.") as User,
                                Consultant = OleDbHelper.MapReaderRecord(consultant, consultant.GetType(), reader, "uconsultant.") as User
                            };

                            result.Add(clientConsultant);
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
            var model = (ClientConsultant)obj;

            try
            {
                OleDbCommand command = null;
                command = new OleDbCommand(string.Format("INSERT INTO [ClientConsultant] ([VendorClientId], [VendorConsultantId], [CreatedDTS], [UpdatedDTS]) VALUES({0}, {1}, {2}, {3})", model.Client.id, model.Consultant.id, "#" + DateTime.Now.ToUniversalTime() + "#", "#" + DateTime.Now.ToUniversalTime() + "#"), connection);
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
            var model = new ClientConsultant() { id = id };
            var result = true;

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.ClientConsultant, OleDbHelper.CommandType.DELETE), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public List<ClientConsultant> GetClientsByConsultant(int id)
        {
            var model = new ClientConsultant() { id = id };
            var result = new List<ClientConsultant>();

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                string sql = string.Format("SELECT cc.id AS ClientConsultantId, uclient.* FROM (((ClientConsultant cc INNER JOIN VendorConsultant vconsultant ON cc.VendorConsultantId = vconsultant.ID) INNER JOIN VendorClient vclient ON cc.VendorClientId = vclient.ID) INNER JOIN [User] uclient ON uclient.id = vclient.ClientId) WHERE (vconsultant.ConsultantId = {0})", id); 
                OleDbCommand command = new OleDbCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var client = new User();

                            var clientConsultant = new ClientConsultant()
                            {
                                id = reader.GetInt32(reader.GetOrdinal("ClientConsultantId")),
                                Client = OleDbHelper.MapReaderRecord(client, client.GetType(), reader, "") as User
                            };

                            result.Add(clientConsultant);
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

        public IModel Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
