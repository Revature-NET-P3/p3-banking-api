using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Banking.API.Controllers;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestAccountController
    {
        // TODO: Swap out object for Test AccountRepo, when implemented.
        object testAccountRepo = null;
        Mock<ILogger<AccountsController>> testLogger = null;
        AccountsController testAccountController = null;

        [TestInitialize]
        public void BeforeEachTest()
        {
            // Generate mock Logger.
            testLogger = new Mock<ILogger<AccountsController>>();

            // Generate testAccountRepo.
            testAccountRepo = new object();

            // Generate controller
            // TODO: Update following injection when functionality completed:
            //testAccountController = new AccountsController(testAccountRepo, testLogger.Object);
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
            var response = testAccountController.GetAllAccountsByUserID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(-1);
            response.Wait(1);
            var resultValue = response.Result.Value;
            
            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserID_InvalidID()
        {
            // Arrange.
            // TODO: set user credentials to different user.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(3);
            response.Wait(1);
            var resultValue = response.Result.Value;
            
            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserID_NoAccountsAccociatedWithID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(2);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserID_ServerError()
        {
            // Arrange.
            // TODO: redefine testAccountRepo to be empty.
            // TODO: reinject testAccountController.

            // Act.
            var response = testAccountController.GetAllAccountsByUserID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ValidIDAndValidAccountType()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(1, 1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(-1, 1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_InvalidID()
        {
            // Arrange.
            // TODO: set user credientals to different user.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(3, 1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ValidIDAndNoValidAccounts()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(2, 4);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllAccountsByUserIDAndAccountType_ServerError()
        {
            // Arrange.
            // TODO: redefine testAccountRepo to be empty.
            // TODO: reinject testAccountController.

            // Act.
            var response = testAccountController.GetAllAccountsByUserIDAndTypeID(1, 1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_ValidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(-1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAccountDetailsByAccountID_InvalidUser()
        {
            // Arrange.
            // TODO: set user credientals to different user.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(3);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }


        [TestMethod]
        public void GetAccountDetailsByAccountID_ServerError()
        {
            // Arrange.
            // TODO: redefine testAccountRepo to be empty.
            // TODO: reinject testAccountController.

            // Act.
            var response = testAccountController.GetAccountDetailsByAccountID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_ValidID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_NonExistingID()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(-1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_InvalidUser()
        {
            // Arrange.
            // TODO: set user credientals to different user.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(3);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetTransactionDetailsByAccountID_ServerError()
        {
            // Arrange.
            // TODO: redefine testAccountRepo to be empty.
            // TODO: reinject testAccountController.

            // Act.
            var response = testAccountController.GetTransactionDetailsByAccountID(1);
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllTransactionTypes_ValidData()
        {
            // Arrange.

            // Act.
            var response = testAccountController.GetAllTransactionTypes();
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllTransactionTypes_EmptyDataSet()
        {
            // Arrange.
            // TODO: Empty TransactionType list in repository testAccountRepo.

            // Act.
            var response = testAccountController.GetAllTransactionTypes();
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }

        [TestMethod]
        public void GetAllTransactionTypes_ServerError()
        {
            // Arrange.
            // TODO: redefine testAccountRepo to be empty.
            // TODO: reinject testAccountController.

            // Act.
            var response = testAccountController.GetAllTransactionTypes();
            response.Wait(1);
            var resultValue = response.Result.Value;

            // Assert.
        }
    }
}