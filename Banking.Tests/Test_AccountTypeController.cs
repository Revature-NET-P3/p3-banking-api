using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using Banking.API.Controllers;
using Banking.API.Models;
using Banking.Tests.DataObjects;

namespace Banking.Tests
{
    [TestClass]
    public class Test_AccountTypeController
    {
        NullLogger<AccountTypesApiController> testLogger = new NullLogger<AccountTypesApiController>();

        List<AccountType> test_accountTypes = new List<AccountType>()
        {
            new AccountType{ Id = 1 , Name = "Checking" , InterestRate = .008M },
            new AccountType{ Id = 2 , Name = "Business" , InterestRate = .02M },
            new AccountType{ Id = 3 , Name = "Loan" , InterestRate = .01M },
            new AccountType{ Id = 4 , Name = "Term Deposit" , InterestRate = .01M }
        };

        [TestMethod]
        public void GetAccountTypes()
        {
            #region Assign
            AccountTypeRepo testRepo = new AccountTypeRepo();
            AccountTypesApiController testController = new AccountTypesApiController(testRepo, testLogger);
            #endregion

            #region Act
            var taskReturn = testController.GetAccountTypes();
            var taskResults = taskReturn.Result.Value.ToList();
            #endregion

            #region Assert
            foreach (AccountType element in taskResults)
            {
                Assert.AreEqual(test_accountTypes[taskResults.IndexOf(element)].Id, element.Id);
                Assert.AreEqual(test_accountTypes[taskResults.IndexOf(element)].Name, element.Name);
                Assert.AreEqual(test_accountTypes[taskResults.IndexOf(element)].InterestRate, element.InterestRate);
            }
            #endregion
        }

        [TestMethod]
        public void GetAccountTypeById()
        {
            #region Assign
            AccountTypeRepo testRepo = new AccountTypeRepo();
            AccountTypesApiController testController = new AccountTypesApiController(testRepo, testLogger);
            #endregion

            foreach (AccountType element in test_accountTypes)
            {
                #region Act
                var taskReturn = testController.GetAccountTypeById(element.Id);
                var taskResult = taskReturn.Result.Value;
                #endregion

                #region Assert
                Assert.AreEqual(element.Id, taskResult.Id);
                Assert.AreEqual(element.Name, taskResult.Name);
                Assert.AreEqual(element.InterestRate, taskResult.InterestRate);
                #endregion
            }
        }

        [TestMethod]
        public void GetAccountTypeByName()
        {
            #region Assign
            AccountTypeRepo testRepo = new AccountTypeRepo();
            AccountTypesApiController testController = new AccountTypesApiController(testRepo, testLogger);
            #endregion

            foreach (AccountType element in test_accountTypes)
            {
                #region Act
                var taskReturn = testController.GetAccountTypeByName(element.Name);
                var taskResult = taskReturn.Result.Value;
                #endregion

                #region Assert
                Assert.AreEqual(element.Id, taskResult.Id);
                Assert.AreEqual(element.Name, taskResult.Name);
                Assert.AreEqual(element.InterestRate, taskResult.InterestRate);
                #endregion
            }
        }

        [TestMethod]
        public void AddAccountType()
        {
            #region Assign
            AccountTypeRepo testRepo = new AccountTypeRepo();
            AccountTypesApiController testController = new AccountTypesApiController(testRepo, testLogger);
            #endregion

            #region Act
            var task = testRepo.AddAccountType(new AccountType { Id = 5, Name = "DankMemes", InterestRate = 69M });
            task.Wait();

            AccountType newAccount = testRepo.GetAccountTypeById(5).Result;
            #endregion

            #region Assert
            Assert.AreEqual(5, newAccount.Id);
            Assert.AreEqual("DankMemes", newAccount.Name);
            Assert.AreEqual(69M, newAccount.InterestRate);
            #endregion
        }
    }
}