using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControllerTests
{
    private readonly ILogger<HomeController> _logger;
    private readonly IServerConfigReader _serverConfigReader;
    private readonly IServerConfigWriter _serverConfigWriter;

    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void SampleTest()
        {
            Assert.AreEqual("HomeController", "HomeController");
        }

        [TestMethod]
        public void ReturnsDetailsView()
        {

            HomeController controllerUnderTest = new HomeController();
            var result = controllerUnderTest.Details("a1") as ViewResult;
            Assert.AreEqual("fooview", result.ViewName);
        }
    }
}
