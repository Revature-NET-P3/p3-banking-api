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
        [DataRow(10,1)]
        [DataRow(20,1)]
        [DataRow(30,2)]
        public void GetAllAccountsByUserID_ValidID(int userID, int accountCount)
        {
            // Arrange.

            // Act. 
            var response = testAccountController.GetAllAccountsByUserID(userID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreEqual(responseValue.Count, accountCount, string.Format("Return List count NOT equal to {0}", accountCount.ToString()));
        }

        [TestMethod]
        public void GetAllAccountsByUserID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(-1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 404 Not Found!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1, string.Format("Return value not {0}", (-1).ToString()));
        }

        [TestMethod]
        [DataRow(10, 2)]
        [DataRow(20, 2)]
        [DataRow(30, 1)]
        public void GetAllAccountsByUserID_InvalidID(int userID, int accountCount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(30);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreNotEqual(responseValue.Count, userID, string.Format("Return List count is equal to {0}", accountCount.ToString()));
        }

        [TestMethod]
        public void GetAllAccountsByUserID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(10);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }

        [TestMethod]
        [DataRow(10,1,3)]
        [DataRow(20,2,1)]
        [DataRow(30,3,2)]
        public void GetAllAccountsByUserIDAndAccountType_ValidIDAndValidAccountType(int userID, int accountID, int accountTypeID)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(userID, accountTypeID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreEqual(responseValue[0].Id, accountID, string.Format("Account ID NOT equal to {0}", accountID.ToString()));
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(-1, 1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 404 Not Found!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1, string.Format("Return value not {0}", (-1).ToString()));
        }

        [TestMethod]
        [DataRow(10, 2, 3)]
        [DataRow(20, 3, 1)]
        [DataRow(30, 1, 2)]
        public void GetAllAccountsByUserIDAndAccountType_InvalidID(int userID, int accountID, int accountTypeID)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(userID, accountTypeID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Account>;

            Assert.AreNotEqual(responseValue[0].Id, accountID, string.Format("Return account ID equal to {0}", accountID.ToString()));
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(1, 1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }

        [TestMethod]
        [DataRow(1,200.0f)]
        [DataRow(2,300.0f)]
        [DataRow(3,500.0f)]
        [DataRow(4,600.0f)]
        public void GetAccountDetailsByAccountID_ValidID(int accountID, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(accountID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as Account;
            Assert.AreEqual(responseValue.Balance, (decimal)amount, string.Format("Return account amount NOT equal to {0}", amount.ToString()));

        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(-1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 404 Not Found!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1, string.Format("Return value not {0}", (-1).ToString()));
        }

        [TestMethod]
        [DataRow(1, 400.0f)]
        [DataRow(2, 200.0f)]
        [DataRow(3, 300.0f)]
        [DataRow(4, 500.0f)]
        public void GetAccountDetailsByAccountID_InvalidAccount(int accountID, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(accountID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as Account;
            Assert.AreNotEqual(responseValue.Balance, (decimal)amount, string.Format("Return account amount equal to {0}", amount.ToString()));
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }

        [TestMethod]
        [DataRow(1, 200.0f)]
        [DataRow(2, 300.0f)]
        [DataRow(3, 200.0f)]
        [DataRow(4, 600.0f)]
        public void GetTransactionDetailsByAccountID_ValidID(int accountID, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(accountID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreEqual(responseValue[0].Ammount, (decimal)amount, string.Format("Return transaction amount NOT equal to {0}", amount.ToString()));
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(-1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 404 Not Found!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, -1, string.Format("Return value not {0}", (-1).ToString()));
        }

        [TestMethod]
        [DataRow(1, 600.0f)]
        [DataRow(2, 200.0f)]
        [DataRow(3, 300.0f)]
        [DataRow(4, 200.0f)]
        public void GetTransactionDetailsByAccountID_InvalidAccountID(int accountID, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(accountID);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreNotEqual(responseValue[0].Ammount, (decimal)amount, string.Format("Return Transaction Amount equal to {0}", amount.ToString()));
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }
    }
}