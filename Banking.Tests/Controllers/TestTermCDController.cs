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
        public void TestValidWithdraw()
        {
            Account termTest = new Account();
            termTest.AccountTypeId = 4;
            termTest.Balance = 1000;
            termTest.CreateDate = new System.DateTime(3/10/2015);
            decimal withdrawAmmount = 500.50m;
            decimal expectedBalance = 499.50m;


            testTermCDController.Withdraw(termTest, withdrawAmmount);
            Assert.AreEqual(termTest.Balance, expectedBalance);
        }

        [TestMethod]
        public void TestInvalidWithdraw()
        {
            Account termTest = new Account();
            termTest.AccountTypeId = 4;
            termTest.Balance = 1000;
            termTest.CreateDate = new System.DateTime(3/10/2015);
            decimal withdrawAmmount = 9999.99m;
            decimal expectedBalance = 1000m;


            testTermCDController.Withdraw(termTest, withdrawAmmount);
            Assert.AreEqual(termTest.Balance, expectedBalance);
        }

        [TestMethod]
        public void TestValidTransfer()
        {
            Account termTest = new Account();
            termTest.AccountTypeId = 4;
            termTest.Balance = 1000;
            termTest.CreateDate = new System.DateTime(3 / 10 / 2015);
            Account otherTest = new Account();
            otherTest.Balance = 1500;
            decimal transferAmmount = 250m;
            decimal expectedBalance = 750m;
            decimal otherExpectedBalance = 1750;

            testTermCDController.Transfer(termTest, otherTest, transferAmmount);
            Assert.AreEqual(termTest.Balance, expectedBalance);
            Assert.AreEqual(otherTest.Balance, otherExpectedBalance);
        }

        [TestMethod]
        public void TestInvalidTransfer()
        {
            Account termTest = new Account();
            termTest.AccountTypeId = 4;
            termTest.Balance = 1000;
            termTest.CreateDate = new System.DateTime(3 / 10 / 2015);
            Account otherTest = new Account();
            otherTest.Balance = 1500;
            decimal transferAmmount = 9999m;
            decimal expectedBalance = 1000m;
            decimal otherExpectedBalance = 1500;

            testTermCDController.Transfer(termTest, otherTest, transferAmmount);
            Assert.AreEqual(termTest.Balance, expectedBalance);
            Assert.AreEqual(otherTest.Balance, otherExpectedBalance);
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
