using System;
using System.Data.OleDb;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Access
{
    public class UserRepository : IUserRepository
    {
        static OleDbConnection connection;

        public UserRepository()
        {
            OleDbHelper.GetConnection(ref connection);
        }

        public Model.IModel GetByEmail(string email)
        {
            IModel model = new User();

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.User, OleDbHelper.CommandType.SELECT_WITH_WHERE, string.Format("Email = '{0}'", email)), connection);
                model = (User)OleDbHelper.PopulateModel(model, command);
                connection.Close();
            }
            catch
            {

            }

            return model;
        }

        public Model.IModel Get(int id)
        {
            IModel model = new User() { id = id };

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.User, OleDbHelper.CommandType.SELECT), connection);
                model = (User)OleDbHelper.PopulateModel(model, command);
                connection.Close();
            }
            catch
            { 

            }

            return model;
        }

        public int Save(object obj)
        {
            User model = (User)obj;

            if (model.id > 0 && (string.IsNullOrEmpty(model.Password) || model.Type == UserType.NotSet))
            {
                var user = Get(model.id) as User;

                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = user.Password;
                }

                if (model.Type == UserType.NotSet)
                {
                    model.Type = user.Type;
                }
            }            

            try
            {
                OleDbCommand command = null;
                if (model.id == 0)
                {
                    command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.User, OleDbHelper.CommandType.INSERT), connection);
                }
                else
                {
                    command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.User, OleDbHelper.CommandType.UPDATE), connection);
                }
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
            IModel model = new User() { id = id };
            var result = true;

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(OleDbHelper.GetSQL(model, Entity.User, OleDbHelper.CommandType.DELETE), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                result = false;
            }

            return result;
        }


        public void StoreToken(Token token)
        {
            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(String.Format("INSERT INTO [SecurityToken] (Token, UserId) VALUES('{0}', {1})", token.Guid.ToString(), token.UserId), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {

            }
        }

        public Token GetToken(Guid guid)
        {
            var token = new Token() { Guid = guid };

            try
            {
                OleDbHelper.OpenConnection(ref connection);
                OleDbCommand command = new OleDbCommand(String.Format("SELECT * FROM [SecurityToken] WHERE Token = '{0}'", guid), connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        token.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return token;
        }
    }
}
