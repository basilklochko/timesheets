using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheet.Library.Model;

namespace Timesheet.Library.Repository.Mssql
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository()
            : base()
        {

        }

        public IModel GetByEmail(string email)
        {
            IModel model = new Model.User();

            var result = (from u in entities.User
                          where u.Email == email
                          select u).FirstOrDefault();

            if (result != null)
            {
                model = DbHelper.UserFromDb(result);
            }

            return model;
        }

        public void StoreToken(Token token)
        {
            var securityToken = DbHelper.TokenToDb(token);

            entities.SecurityToken.Add(securityToken);
            entities.SaveChanges();
        }

        public Token GetToken(Guid guid)
        {
            var result = new Token();

            var securityToken = (from t in entities.SecurityToken
                     where t.Token == guid.ToString()
                     select t).FirstOrDefault();

            if (securityToken != null)
            {
                result = DbHelper.TokenFromDb(securityToken);
            }

            return result;
        }

        public int Save(object obj)
        {
            Model.User model = (Model.User)obj;
            User existingUser = (from u in entities.User
                                 where u.id == model.id
                                 select u).FirstOrDefault();


            if (model.id > 0 && (string.IsNullOrEmpty(model.Password) || model.Type == UserType.NotSet))
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    model.Password = existingUser.Password;
                }

                if (model.Type == UserType.NotSet)
                {
                    model.Type = (UserType)existingUser.Type;
                }
            }

            try
            {
                if (model.id == 0)
                {
                    existingUser = DbHelper.UserToDb(model, null);
                    existingUser.CreatedDTS = DateTime.Now.ToUniversalTime();
                    entities.User.Add(existingUser);
                }
                else
                {
                    existingUser = DbHelper.UserToDb(model, existingUser);
                }

                existingUser.UpdatedDTS = DateTime.Now.ToUniversalTime();
                entities.SaveChanges();

                return existingUser.id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IModel Get(int id)
        {
            IModel model = new Model.User();

            var result = (from u in entities.User
                          where u.id == id
                          select u).FirstOrDefault();

            if (result != null)
            {
                model = DbHelper.UserFromDb(result);
            }

            return model;
        }

        public bool Delete(int id)
        {
            var result = (from u in entities.User
                          where u.id == id
                          select u).FirstOrDefault();

            if (result != null)
            {
                entities.User.Remove(result);
                entities.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
