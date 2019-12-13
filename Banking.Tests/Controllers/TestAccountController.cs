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
            var response = testAccountController.GetAllAccountsByUserID(userID);
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

        [TestMethod]
        [DataRow(1, "1-1-2000", "1-1-2002", 200.0f)]
        [DataRow(2, "1-1-2000", "1-1-2001", 300.0f)]
        [DataRow(3, "1-1-1990", "11-1-1990", 200.0f)]
        [DataRow(4, "4-4-2002", "6-6-2002", 600.0f)]
        public void GetTransactionDetailsByAccountIDWithDateRange_ValidResponse(int accountID, string start, string end, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithDateRange(accountID, start, end);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreEqual(responseValue[0].Ammount, (decimal)amount, string.Format("Return Transaction Amount NOT equal to {0}", amount.ToString()));
        }

        [TestMethod]
        [DataRow(1, "1-1-2000", "12-1-2000")]
        [DataRow(2, "1-1-2000", "2-1-2000")]
        [DataRow(3, "1-1-2000", "11-1-2000")]
        [DataRow(4, "4-4-2003", "6-6-2003")]
        public void GetTransactionDetailsByAccountIDWithDateRange_NoResultDateRange(int accountID, string start, string end)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithDateRange(accountID, start, end);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 200 OK!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, accountID, string.Format("Return value is NOT equal to {0}", accountID.ToString()));
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountIDWithDateRange_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithDateRange(1, "1-1-2000", "12-1-2000");
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }

        [TestMethod]
        [DataRow(1,1,200.0f)]
        [DataRow(2,1,300.0f)]
        [DataRow(3,1,200.0f)]
        [DataRow(4,1,600.0f)]
        public void GetTransactionDetailsByAccountIDWithLimit_ValidResponse(int accountID, int limit, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimit(accountID, limit);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreEqual(responseValue.Count, 1, string.Format("Transaction limit list for Account {0} NOT equal to {1}", accountID.ToString(), limit));
            Assert.AreEqual(responseValue[0].Ammount, (decimal)amount, string.Format("Return Transaction Amount NOT equal to {0}", amount.ToString()));
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        [DataRow(4, 0)]
        public void GetTransactionDetailsByAccountIDWithLimit_NoResultLimit(int accountID, int limit)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimit(accountID, limit);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 200 OK!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, accountID, string.Format("Return value is NOT equal to {0}", accountID.ToString()));
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountIDWithLimit_ServerError()
        {
            // Arrange.
            testAccountRepo = new AccountRepoTest(false);
            testAccountController = new AccountsController(testAccountRepo, testLogger.Object);

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimit(1, 1);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(ObjectResult), "HTTP Response NOT an ObjectResult!");
            Assert.AreEqual((responseResult as ObjectResult).StatusCode, 500, "HTTP Response status code NOT 500!");
        }

        [TestMethod]
        [DataRow(1, 2, "1-1-2000", "1-1-2002", 1, 200.0f)]
        [DataRow(2, 2, "1-1-2000", "1-1-2001", 1, 300.0f)]
        [DataRow(3, 2, "1-1-1990", "12-1-1990", 2, 200.0f)]
        [DataRow(4, 2, "4-4-2002", "6-6-2002", 1, 600.0f)]
        public void GetTransactionDetailsByAccountIDWithLimitAndDateRange_ValidResponse(int accountID, int limit, string start, string end, int listCount, float amount)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimitAndDateRange(accountID, limit, start, end);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(OkObjectResult), "HTTP Response NOT 200 OK!");
            var responseValue = (responseResult as OkObjectResult).Value as List<Transaction>;
            Assert.AreEqual(responseValue.Count, listCount, string.Format("Return list size NOT equal to {0}", listCount.ToString()));
            Assert.AreEqual(responseValue[0].Ammount, (decimal)amount, string.Format("Return Transaction Amount NOT equal to {0}", amount.ToString()));
        }

        [TestMethod]
        [DataRow(1, "1-1-2000", "12-1-2000")]
        [DataRow(2, "1-1-2000", "2-1-2000")]
        [DataRow(3, "1-1-2000", "11-1-2000")]
        [DataRow(4, "4-4-2003", "6-6-2003")]
        public void GetTransactionDetailsByAccountIDWithLimitAndDateRange_NoResultDateRange(int accountID, string start, string end)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimitAndDateRange(accountID, 1, start, end);
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 200 OK!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, accountID, string.Format("Return value is NOT equal to {0}", accountID.ToString()));
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        [DataRow(4, 0)]
        public void GetTransactionDetailsByAccountIDWithLimitAndDateRange_NoResultLimit(int accountID, int limit)
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountIDWithLimitAndDateRange(accountID, limit, "1-1-1950", "12-31-3000");
            response.Wait(500);
            var responseResult = response.Result.Result;

            // Assert.
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 200 OK!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, accountID, string.Format("Return value is NOT equal to {0}", accountID.ToString()));
        }

       
    }
}