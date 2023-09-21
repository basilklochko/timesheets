using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timesheet.Library.Repository;
using Timesheet.Library.Repository.Access;
using Timesheet.Library.Model;

namespace Timesheet.Test.Repository.Access
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void Insert_Test()
        {
            var result = 0;

            User user = new User()
            {
                Type = UserType.Consultant,
                UserName = "Vasyl Klochko",
                Email = "v@t.com",
                Password = "1"
            };
            
            result = new UserRepository().Save(user);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Update_Test()
        {
            var result = 2;

            User user = new UserRepository().Get(2) as User;
            user.Phone = "1111-2222";
            user.UpdatedDTS = DateTime.Now;

            result = new UserRepository().Save(user);
            Assert.IsTrue(result == 2);
        }

        [TestMethod]
        public void Get_Test()
        {
            var result = new UserRepository().Get(2);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByEmail_Test()
        {
            var result = new UserRepository().GetByEmail("v1@t.com");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_Test()
        {
            var result = new UserRepository().Delete(4);
            Assert.IsTrue(result);
        }
    }
}
