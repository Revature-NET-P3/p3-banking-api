using Banking.API.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestAccountController
    {
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
        }

        [TestMethod]
        public void GetAllAccountsByUserID_NonExistingID()
        {

        }

        [TestMethod]
        public void GetAllAccountsByUserID_InvalidID()
        {

        }

        [TestMethod]
        public void GetAllAccountsByUserID_NoAccounts()
        {

        }
    }
}
