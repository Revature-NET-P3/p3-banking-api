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
        IAccountRepo _repoAccount;
        Mock<ILogger<TransferablesController>> transferableLogger;
        TransferablesController testTransferables;
    }

}
