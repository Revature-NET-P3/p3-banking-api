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
    public class TestTermCDController
    {
        AccountRepoTest testAccountRepo = null;
        Mock<ILogger<AccountsController>> testLogger = null;
        TermCDController testTermCDController = null;

        [TestInitialize]
        public void BeforeEachTest()
        {
            // Generate mock Logger.
            testLogger = new Mock<ILogger<AccountsController>>();

            // Generate testAccountRepo.
            testAccountRepo = new AccountRepoTest();

            // Generate controller
            // TODO: Update following injection when functionality completed:
            testTermCDController = new TermCDController(testAccountRepo);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            testLogger = null;
            testAccountRepo = null;
            testTermCDController = null;
        }

        [TestMethod]
        public  void TestAddAccount_ValidTermCDId()
        {
            Account addThis = new Account();
            addThis.AccountTypeId = 4;

            var response = testTermCDController.AddTermCD(addThis);
            response.Wait(1);
            var responseResult = response.Result;

            Assert.IsInstanceOfType(responseResult, typeof(OkResult));

        }

        [TestMethod]
        public void TestAddAccount_InvalidTermCDId()
        {
            Account addThis = new Account();
            addThis.AccountTypeId = 3;

            var response = testTermCDController.AddTermCD(addThis);
            response.Wait(1);
            var responseResult = response.Result;

            Assert.IsInstanceOfType(responseResult, typeof(BadRequestResult));
        }
    }
}
