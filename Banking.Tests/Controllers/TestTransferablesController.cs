using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Banking.API.Controllers;
using Banking.Tests.DataObjects;
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestTransferablesController
    {

        [TestMethod]
        public void CreateAccount()
        {
            //Arrange
            var id = 12;
            var userId = 5;
            var accountType = 1;
            var balance = 15.50M;
            var createDate = new DateTime(2019, 12, 25);
            var isClose = false;
            //set up mock
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            var newAccount = new Account() {Id = id, UserId = userId,AccountTypeId = accountType, Balance = balance, CreateDate = createDate, IsClosed = isClose};
            // Generate controller
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Post(newAccount);
            var acct = acctResponse.Result.Result as CreatedAtActionResult;
            var crAcct = acct.Value as Account;
            //Assert
            Assert.AreSame(newAccount, crAcct);
            Assert.AreEqual(userId, crAcct.UserId);
            Assert.AreEqual(accountType, crAcct.AccountTypeId);
            Assert.AreEqual(createDate, crAcct.CreateDate);
            Assert.AreEqual(isClose, crAcct.IsClosed);

        }

        [TestMethod]
        public void CloseAccount()
        {
            //Arrange
            var id = 5;
            var isClose = true; //should be true after closing
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();
            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Close(id);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(isClose, acct.IsClosed);
            Assert.AreEqual(id, acct.Id);
        }

        [TestMethod]
        public void CloseAccount_Funds()
        {
            //Arrange
            var id = 2;
            var isClose = false; //should be false because funds in account 
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();
            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Close(id);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(isClose, acct.IsClosed);
            Assert.AreEqual(id, acct.Id);
        }

        [TestMethod]
        public void DepositAccount()
        {
            //Arrange
            var balanceAfter = 400M; //Balance of account with id 2 is 300 before deposit
            var amount = 100;
            var id = 2;
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Deposit(id,amount);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfter, acct.Balance);
            Assert.AreEqual(id, acct.Id);
        }

        [TestMethod]
        public void DepositAccount_InvalidID()
        {
            //Arrange
            //var balanceAfter = 300M; //Balance of account with id 2 is 300 before deposit
            var amount = 100;
            var id = 10;
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Deposit(id, amount);
            acctResponse.Wait(500);
  
            var responseResult = acctResponse.Result;
            Assert.IsInstanceOfType(responseResult, typeof(NotFoundObjectResult), "HTTP Response NOT 404 Not Found!");
            Assert.AreEqual((responseResult as NotFoundObjectResult).Value, 10, string.Format("Return value not {0}", (-1).ToString()));
        }

        [TestMethod]
        public void WithdrawAccount()
        {
            //Arrange
            var balanceAfter = 200M; //Balance of account with id 2 is 300 before deposit
            var amount = 100;
            var id = 2;
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Withdraw(id, amount);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfter, acct.Balance);
            Assert.AreEqual(id, acct.Id);

        }


        [TestMethod]
        public void WithdrawAccountBusiness_Overdraft()
        {
            //Arrange
            var balanceAfter = -102M; //Balance of account with id 3 is 500 before deposit
            var amount = 600;
            var id = 3;
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Withdraw(id, amount);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfter, acct.Balance);
            Assert.AreEqual(id, acct.Id);

        }

        [TestMethod]
        public void WithdrawAccountChecking_Overdraft()
        {
            //Arrange
            var balanceAfter = 300M; //Balance of account with id 2 is 300 before deposit
            var amount = 400;
            var id = 2;
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Withdraw(id, amount);
            acctResponse.Wait(500);
            var response = testAccountController.GetAccountDetailsByAccountID(id);
            response.Wait(500);
            var responseResult = response.Result.Result;
            var acct = (responseResult as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfter, acct.Balance);
            Assert.AreEqual(id, acct.Id);

        }

        [TestMethod]
        public void TransferFromChecking()
        {
            //Arrange
            var balanceAfterFromAccount = 200M; //Balance of account with id 2 is 300 before deposit
            var balanceAfterToAccount = 600M; //Balance of account with id 3 is 500 before deposit
            var amount = 100;
            var idFrom = 2; //checking account
            var idTo = 3; //bussiness account
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Transfer(idFrom, idTo, amount);
            acctResponse.Wait(500);
            var responseFrom = testAccountController.GetAccountDetailsByAccountID(idFrom);
            responseFrom.Wait(500);
            var responseTo = testAccountController.GetAccountDetailsByAccountID(idTo);
            responseTo.Wait(500);

            var responseResult = responseFrom.Result.Result;
            var responseResult2 = responseTo.Result.Result;
            var acctFrom = (responseResult as OkObjectResult).Value as Account;
            var acctTo = (responseResult2 as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfterFromAccount, acctFrom.Balance);
            Assert.AreEqual(idFrom, acctFrom.Id);
            Assert.AreEqual(balanceAfterToAccount, acctTo.Balance);
            Assert.AreEqual(idTo, acctTo.Id);

        }

        [TestMethod]
        public void TransferFromChecking_Overdraft() //should fail to transfer
        {
            //Arrange
            var balanceAfterFromAccount = 300M; //Balance of account with id 2 is 300 before deposit
            var balanceAfterToAccount = 500M; //Balance of account with id 3 is 500 before deposit
            var amount = 350; //try to overdraft checking account
            var idFrom = 2; //checking account
            var idTo = 3; //bussiness account
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Transfer(idFrom, idTo, amount);
            acctResponse.Wait(500);
            var responseFrom = testAccountController.GetAccountDetailsByAccountID(idFrom);
            responseFrom.Wait(500);
            var responseTo = testAccountController.GetAccountDetailsByAccountID(idTo);
            responseTo.Wait(500);

            var responseResult = responseFrom.Result.Result;
            var responseResult2 = responseTo.Result.Result;
            var acctFrom = (responseResult as OkObjectResult).Value as Account;
            var acctTo = (responseResult2 as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfterFromAccount, acctFrom.Balance);
            Assert.AreEqual(idFrom, acctFrom.Id);
            Assert.AreEqual(balanceAfterToAccount, acctTo.Balance);
            Assert.AreEqual(idTo, acctTo.Id);

        }

        [TestMethod]
        public void TransferFromBusiness_Overdraft() //should succesfully transfer
        {
            //Arrange
            var balanceAfterFromAccount = -102M; //Balance of account with id 3 is 500 before deposit
            var balanceAfterToAccount = 900M; //Balance of account with id 2 is 300 before deposit
            var amount = 600;
            var idFrom = 3; //business account
            var idTo = 2; //checking account
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Transfer(idFrom, idTo, amount);
            acctResponse.Wait(500);
            var responseFrom = testAccountController.GetAccountDetailsByAccountID(idFrom);
            responseFrom.Wait(500);
            var responseTo = testAccountController.GetAccountDetailsByAccountID(idTo);
            responseTo.Wait(500);

            var responseResult = responseFrom.Result.Result;
            var responseResult2 = responseTo.Result.Result;
            var acctFrom = (responseResult as OkObjectResult).Value as Account;
            var acctTo = (responseResult2 as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfterFromAccount, acctFrom.Balance);
            Assert.AreEqual(idFrom, acctFrom.Id);
            Assert.AreEqual(balanceAfterToAccount, acctTo.Balance);
            Assert.AreEqual(idTo, acctTo.Id);

        }

        [TestMethod]
        public void TransferFromLoan() //should fail to transfer
        {
            //Arrange
            var balanceAfterFromAccount = 200M; //Balance of account with id 1 is 200 before transfer
            var balanceAfterToAccount = 300M; //Balance of account with id 2 is 300 before transfer
            var amount = 600;
            var idFrom = 1; //Loan account
            var idTo = 2; //checking account
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Transfer(idFrom, idTo, amount);
            acctResponse.Wait(500);
            var responseFrom = testAccountController.GetAccountDetailsByAccountID(idFrom);
            responseFrom.Wait(500);
            var responseTo = testAccountController.GetAccountDetailsByAccountID(idTo);
            responseTo.Wait(500);

            var responseResult = responseFrom.Result.Result;
            var responseResult2 = responseTo.Result.Result;
            var acctFrom = (responseResult as OkObjectResult).Value as Account;
            var acctTo = (responseResult2 as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfterFromAccount, acctFrom.Balance);
            Assert.AreEqual(idFrom, acctFrom.Id);
            Assert.AreEqual(balanceAfterToAccount, acctTo.Balance);
            Assert.AreEqual(idTo, acctTo.Id);

        }

        [TestMethod]
        public void TransferFromCD() //should fail to transfer
        {
            //Arrange
            var balanceAfterFromAccount = 600M; //Balance of account with id 4 is 600 before transfer
            var balanceAfterToAccount = 300M; //Balance of account with id 2 is 300 before transfer
            var amount = 600;
            var idFrom = 4; //CD account
            var idTo = 2; //checking account
            var transferableLogger = new Mock<ILogger<TransferablesController>>();
            var accountLogger = new Mock<ILogger<AccountsController>>();

            var accountMock = new AccountRepoTest();
            var accountTypeMock = new AccountTypeRepo();
            // Generate controller
            var testAccountController = new AccountsController(accountMock, accountLogger.Object);
            var testTransferablesController = new TransferablesController(accountMock, accountTypeMock, transferableLogger.Object);
            //Act
            var acctResponse = testTransferablesController.Transfer(idFrom, idTo, amount);
            acctResponse.Wait(500);
            var responseFrom = testAccountController.GetAccountDetailsByAccountID(idFrom);
            responseFrom.Wait(500);
            var responseTo = testAccountController.GetAccountDetailsByAccountID(idTo);
            responseTo.Wait(500);

            var responseResult = responseFrom.Result.Result;
            var responseResult2 = responseTo.Result.Result;
            var acctFrom = (responseResult as OkObjectResult).Value as Account;
            var acctTo = (responseResult2 as OkObjectResult).Value as Account;
            //Assert
            Assert.AreEqual(balanceAfterFromAccount, acctFrom.Balance);
            Assert.AreEqual(idFrom, acctFrom.Id);
            Assert.AreEqual(balanceAfterToAccount, acctTo.Balance);
            Assert.AreEqual(idTo, acctTo.Id);

        }

    }
}
