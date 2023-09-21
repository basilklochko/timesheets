using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Timesheet.Library.Repository;
using Timesheet.Library.Repository.Mssql;
using Timesheet.Library.Model;

namespace Timesheet.Test.Repository.Mssql
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void Insert_Test()
        {
            var result = 0;

            var user = new Timesheet.Library.Model.User()
            {
                Type = UserType.Consultant,
                UserName = "Vasyl Klochko",
                Email = "v@t.com",
                Password = "1",
                CreatedDTS = DateTime.Now,
                UpdatedDTS = DateTime.Now
            };

            result = new UserRepository().Save(user);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Update_Test()
        {
            var result = 3;

            var user = new UserRepository().Get(3) as Timesheet.Library.Model.User;
            user.Phone = "1111-2222";
            user.UpdatedDTS = DateTime.Now;

            result = new UserRepository().Save(user);
            Assert.IsTrue(result == 3);
        }

        [TestMethod]
        public void Get_Test()
        {
            var result = new UserRepository().Get(3);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByEmail_Test()
        {
            var result = new UserRepository().GetByEmail("basil@ukr.net");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_Test()
        {
            var result = new UserRepository().Delete(6);
            Assert.IsTrue(result);
        }
    }
}
