using Microsoft.VisualStudio.TestTools.UnitTesting;

using Banking.API.Controllers;
using Banking.Tests.DataObjects;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestUserAPIController
    {
        [TestMethod]
        public void GetUser_Valid_Id()
        {
            #region Assign
             UserRepoTest testData = new UserRepoTest();
            UserAPIController testController = new UserAPIController(testData);

            #endregion

            #region Act
            var taskReturn = testController.GetUser(1);
            taskReturn.Wait();
            var result = taskReturn.Result.Value;

            #endregion

            #region Assert
            Assert.AreEqual(result.Username, "Swag0");
            Assert.AreEqual(result.Email, "null");


            #endregion
        }

        [TestMethod]
        public void GetUser_InValid_Id()
        {
            #region Assign
            UserRepoTest testData = new UserRepoTest();

            UserAPIController testController = new UserAPIController(testData);

            #endregion

            #region Act
            var taskReturn = testController.GetUser(1);
            taskReturn.Wait();
            var result = taskReturn.Result.Value;

            #endregion

            #region Assert
            Assert.AreNotEqual(result.Username, "Swag1");

            #endregion
        }

        [TestMethod]
        public void GetUserByID_NonExits()
        {
            #region Assign
            UserRepoTest testData = new UserRepoTest();
            UserAPIController testController = new UserAPIController(testData);

            #endregion

            #region  Act

            var taskReturn = testController.GetUser(1);
            taskReturn.Wait();
            var result = taskReturn.Result.Value;
            #endregion

            #region

            Assert.AreEqual(result.Username, "Swag0");

            #endregion
        }
    }
}