using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Banking.API.Controllers;
using Banking.API.Models;
using Banking.Tests.DataObjects;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestTermCDController
    {
        AccountRepoTest testAccountRepo = null;
        Mock<ILogger<TermCDController>> testLogger = null;
        TermCDController testTermCDController = null;

        [TestInitialize]
        public void BeforeEachTest()
        {
            // Generate mock Logger.
            testLogger = new Mock<ILogger<TermCDController>>();

            // Generate testAccountRepo.
            testAccountRepo = new AccountRepoTest();

            // Generate controller
            testTermCDController = new TermCDController(testAccountRepo, testLogger.Object);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            testLogger = null;
            testAccountRepo = null;
            testTermCDController = null;
        }

        [TestMethod]
        public void TestValidWithdraw()
        {
            Account termTest = new Account
            {
                Id = 40,
                AccountTypeId = 4,
                Balance = 1000,
                CreateDate = new System.DateTime(3 / 10 / 2015)
            };
            testAccountRepo._accounts.Add(termTest);
            decimal withdrawAmmount = 500.50m;
            decimal expectedBalance = 499.50m;


            testTermCDController.Withdraw(termTest.Id, withdrawAmmount).Wait(500);
            Assert.AreEqual(termTest.Balance, expectedBalance);
        }

        [TestMethod]
        public void TestInvalidWithdraw()
        {
            Account termTest = new Account
            {
                Id = 40,
                AccountTypeId = 4,
                Balance = 1000,
                CreateDate = new System.DateTime(3 / 10 / 2015)
            };
            testAccountRepo._accounts.Add(termTest);
            decimal withdrawAmmount = 9999.99m;
            decimal expectedBalance = 1000m;


            testTermCDController.Withdraw(termTest.Id, withdrawAmmount).Wait(500);
            Assert.AreEqual(termTest.Balance, expectedBalance);
        }

        [TestMethod]
        public void TestValidTransfer()
        {
            Account termTest = new Account
            {
                Id = 40,
                AccountTypeId = 4,
                Balance = 1000,
                CreateDate = new System.DateTime(3 / 10 / 2015)
            };
            Account otherTest = new Account
            {
                Id = 45,
                Balance = 1500
            };
            testAccountRepo._accounts.Add(termTest);
            testAccountRepo._accounts.Add(otherTest);
            decimal transferAmmount = 250m;
            decimal expectedBalance = 750m;
            decimal otherExpectedBalance = 1750;

            testTermCDController.Transfer(termTest.Id, otherTest.Id, transferAmmount).Wait(500);
            Assert.AreEqual(termTest.Balance, expectedBalance);
            Assert.AreEqual(otherTest.Balance, otherExpectedBalance);
        }

        [TestMethod]
        public void TestInvalidTransfer()
        {
            Account termTest = new Account
            {
                Id = 40,
                AccountTypeId = 4,
                Balance = 1000,
                CreateDate = new System.DateTime(3 / 10 / 2015)
            };
            Account otherTest = new Account
            {
                Id = 45,
                Balance = 1500
            };
            testAccountRepo._accounts.Add(termTest);
            testAccountRepo._accounts.Add(otherTest);
            decimal transferAmmount = 9999m;
            decimal expectedBalance = 1000m;
            decimal otherExpectedBalance = 1500;

            testTermCDController.Transfer(termTest.Id, otherTest.Id, transferAmmount).Wait(500);
            Assert.AreEqual(termTest.Balance, expectedBalance);
            Assert.AreEqual(otherTest.Balance, otherExpectedBalance);
        }

        [TestMethod]
        public  void TestAddAccount_ValidTermCDId()
        {
            Account addThis = new Account
            {
                AccountTypeId = 4
            };

            var response = testTermCDController.AddTermCD(addThis);
            response.Wait(1);
            var responseResult = response.Result;

            Assert.IsInstanceOfType(responseResult, typeof(CreatedAtActionResult));

        }

        [TestMethod]
        public void TestAddAccount_InvalidTermCDId()
        {
            Account addThis = new Account
            {
                AccountTypeId = 3
            };

            var response = testTermCDController.AddTermCD(addThis);
            response.Wait(1);
            var responseResult = response.Result;

            Assert.IsInstanceOfType(responseResult, typeof(BadRequestResult));
        }
    }
}
