using ConfigEditor.Controllers;
using ConfigEditor.Domain;
using ConfigEditor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class HomeControllerTests
    {
        private IServerConfigReader _serverConfigReader;
        private IServerConfigWriter _serverConfigWriter;
        private ILogger<HomeController> _logger;

        private HomeController _homeController;


        [TestMethod]
        public async Task Index_TestWith_DefaultConfig_And_OneServerOverride_Returns_ConfigResults()
        {
            _serverConfigReader = Mock.Of<IServerConfigReader>(MockBehavior.Strict);
            var serverConfig = new ServerConfig
            {
                Defaults = new Dictionary<string, string> { { "default", "test" } },
                Servers = new List<Server>
                {
                    new Server
                    {
                        Name = "srv123",
                        Overrides = new Dictionary<string, string>{ {"server", "test1"} }
                    }
                }
            };

            Mock.Get(_serverConfigReader)
                .Setup(x => x.GetServerConfig()).ReturnsAsync(serverConfig);

            _homeController = new HomeController(_logger, _serverConfigReader, _serverConfigWriter);

            var results = await _homeController.Index() as ViewResult;

            var serverConfigResults = results.Model as ServerConfigViewModel;

            Assert.IsNotNull(results);
            Assert.IsNotNull(serverConfigResults);
            //server asserts
            Assert.IsNotNull(serverConfigResults.Servers);
            Assert.AreEqual(1, serverConfigResults.Servers.Count);
            Assert.AreEqual("srv123", serverConfigResults.Servers[0].Name);
            Assert.IsTrue(serverConfigResults.Servers[0].Overrides.ContainsKey("server"));
            Assert.IsTrue(serverConfigResults.Servers[0].Overrides.ContainsValue("test1"));

            //default asserts
            Assert.IsNotNull(serverConfigResults.Defaults);
            Assert.AreEqual(1, serverConfigResults.Defaults.Count);
            Assert.IsTrue(serverConfigResults.Defaults.ContainsKey("default"));
            Assert.IsTrue(serverConfigResults.Defaults.ContainsValue("test"));
        }

        [TestMethod]
        public async Task Index_TestWith_NoDefaultConfig_And_With_OneServerOverride_Returns_ConfigWithServer()
        {
            _serverConfigReader = Mock.Of<IServerConfigReader>(MockBehavior.Strict);
            var serverConfig = new ServerConfig
            {
                Servers = new List<Server>
                {
                    new Server
                    {
                        Name = "srv123",
                        Overrides = new Dictionary<string, string>{{"some", "something"}}
                    }
                }
            };

            Mock.Get(_serverConfigReader)
                .Setup(x => x.GetServerConfig()).ReturnsAsync(serverConfig);

            _homeController = new HomeController(_logger, _serverConfigReader, _serverConfigWriter);

            var results = await _homeController.Index() as ViewResult;

            var serverConfigResults = results.Model as ServerConfigViewModel;

            Assert.IsNotNull(results);
            Assert.IsNotNull(serverConfigResults);
            Assert.IsNull(serverConfigResults.Defaults);
            Assert.IsNotNull(serverConfigResults.Servers);
            Assert.AreEqual(1, serverConfigResults.Servers.Count);
            Assert.AreEqual("srv123", serverConfigResults.Servers[0].Name);
            Assert.IsTrue(serverConfigResults.Servers[0].Overrides.ContainsKey("some"));
            Assert.IsTrue(serverConfigResults.Servers[0].Overrides.ContainsValue("something"));
        }

        [TestMethod]
        public async Task Index_TestWith_OnlyDefaultConfig_And_NoServerOverride_Returns_ConfigWithDefault()
        {
            _serverConfigReader = Mock.Of<IServerConfigReader>(MockBehavior.Strict);
            var serverConfig = new ServerConfig
            {
                Defaults = new Dictionary<string, string> { { "some", "something" } }
            };

            Mock.Get(_serverConfigReader)
                .Setup(x => x.GetServerConfig()).ReturnsAsync(serverConfig);

            _homeController = new HomeController(_logger, _serverConfigReader, _serverConfigWriter);

            var results = await _homeController.Index() as ViewResult;

            var serverConfigResults = results.Model as ServerConfigViewModel;

            Assert.IsNotNull(results);
            Assert.IsNotNull(serverConfigResults);
            Assert.IsNotNull(serverConfigResults.Servers);
            Assert.IsNotNull(serverConfigResults.Defaults);
            Assert.AreEqual(1, serverConfigResults.Defaults.Count);
            Assert.AreEqual(0, serverConfigResults.Servers.Count);
            Assert.IsTrue(serverConfigResults.Defaults.ContainsKey("some"));
            Assert.IsTrue(serverConfigResults.Defaults.ContainsValue("something"));
        }

        [TestMethod]
        public async Task Index_TestWith_NullConfig_ReturnsBadRequest()
        {
            _serverConfigReader = Mock.Of<IServerConfigReader>(MockBehavior.Strict);
            var serverConfig = new ServerConfig();
            Mock.Get(_serverConfigReader)
                .Setup(x => x.GetServerConfig()).ReturnsAsync(serverConfig);

            serverConfig = null;

            _homeController = new HomeController(_logger, _serverConfigReader, _serverConfigWriter);

            var results = await _homeController.Index();
            var notound = results as NotFoundResult;

            Assert.IsNotNull(results);
            Assert.IsNotNull(notound);
            Assert.AreEqual(404, notound.StatusCode);
        }
    }
}