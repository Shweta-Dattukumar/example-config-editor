using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigEditor.Controllers;
using ConfigEditor.Domain;

namespace TestProject1
{
    [TestClass]
    public class ServerConfigReaderTests
    {
        private ServerConfigReader _serverConfigReader;


        [TestMethod]
        public async Task ServerConfigReader_SuccessTest()
        {
            _serverConfigReader = new ServerConfigReader();

            var result = _serverConfigReader.GetServerConfig();

            Assert.IsNotNull(result);
        }
    }
}
