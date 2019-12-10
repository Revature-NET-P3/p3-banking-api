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
        object testLogger = null;
        
        [TestInitialize]
        public void BeforeEachTest()
        {
            // Generate mock Logger.
            testLogger = new Mock<ILogger<AccountsController>>();

            // Generate testAccountRepo.
            testAccountRepo = new object();
        }
        
        [TestCleanup]
        public void AfterEachTest()
        {
            testLogger = null;
            testAccountRepo = null;
        }

      
    }
}
