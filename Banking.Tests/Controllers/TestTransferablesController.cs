using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using Banking.API.Controllers;
using Banking.Tests.DataObjects;
using Banking.API.Repositories.Interfaces;

namespace Banking.Tests.Controllers
{
    [TestClass]
    public class TestTransferablesController
    {
        //Mock<ILogger<TransferablesController>> _transferableLogger;
        //Mock<IAccountRepo> _accountMock;
        //Mock<IAccountTypeRepo> _accountTypeMock;
        //TransferablesController _testTransferables;

        [TestInitialize]
        public void BeforeEachTest()
        {
            //set up mock
            var _transferableLogger = new Mock<ILogger<TransferablesController>>();
            var _accountMock = new Mock<IAccountRepo>();
            var _accountTypeMock = new Mock<IAccountTypeRepo>();

            // Generate controller
            var _testTransferables = new TransferablesController(_accountMock.Object, _accountTypeMock.Object,_transferableLogger.Object);
        }


    }
}
