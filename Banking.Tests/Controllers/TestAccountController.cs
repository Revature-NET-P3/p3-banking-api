using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Banking.API.Controllers;
using Banking.Tests.DataObjects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Banking.API.Models;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestAccountController
    {
        AccountRepoTest testAccountRepo = null;
        Mock<ILogger<AccountsController>> testLogger = null;
        AccountsController testAccountController = null;

        [TestInitialize]
        public void BeforeEachTest()
        {
            // Generate mock Logger.
            testLogger = new Mock<ILogger<AccountsController>>();

            // Generate testAccountRepo.
            testAccountRepo = new AccountRepoTest();

            // Generate controller
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);
        }
        
        [TestCleanup]
        public void AfterEachTest()
        {
            testLogger = null;
            testAccountRepo = null;
            testAccountController = null;
        }

        [TestMethod]
        public void GetAllAccountsByUserID_ValidID()
        {
            // Arrange.

            // Act. 
            var response = testAccountController.GetAllAccountsByUserID(10);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreEqual(responseValue.Count, 1);
        }

        [TestMethod]
        public void GetAllAccountsByUserID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(-1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult));
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1);

        }

        [TestMethod]
        public void GetAllAccountsByUserID_InvalidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(30);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreNotEqual(responseValue.Count, 1);
        }

        [TestMethod]
        public void GetAllAccountsByUserID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(10);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult));
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500);
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ValidIDAndValidAccountType()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(10, 3);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreEqual(responseValue[0].Id, 1);
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(-1, 1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult));
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1);
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_InvalidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(30, 2);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreNotEqual(responseValue[0].Id, 1);
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(1, 1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult));
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500);
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_ValidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as Account;
            Assert.AreEqual(responseValue.Balance, 200);

        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(-1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult));
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1);
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_InvalidAccount()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(3);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as Account;
            Assert.AreNotEqual(responseValue.Balance, 200);
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult));
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500);
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_ValidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreEqual(responseValue[0].Ammount, 200);
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(-1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult));
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1);
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_InvalidUser()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(2);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult));
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreNotEqual(responseValue[0].Ammount, 200);
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(1);
            response.Wait(1);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult));
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500);
        }
    }
}