using ConfigEditor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ConfigEditor.Domain;

namespace ConfigEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServerConfigReader _serverConfigReader;
        private readonly IServerConfigWriter _serverConfigWriter;

        public HomeController(ILogger<HomeController> logger, IServerConfigReader serverConfigReader, IServerConfigWriter serverConfigWriter)
        {
            _logger = logger;
            _serverConfigReader = serverConfigReader;
            _serverConfigWriter = serverConfigWriter;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _serverConfigReader.GetServerConfig();

            // map model to view 
            var serveConfigViewModel = new ServerConfigViewModel(results);

            if (serveConfigViewModel.Defaults == null && serveConfigViewModel.Servers.Count == 0)
            {
                return NotFound();
            }

            return View(serveConfigViewModel);
        }

        public async Task SaveConfigx(ServerConfigViewModel serverConfigViewModel)
        {
            // map view to model 
            var serverConfig = MapServerConfigViewModel(serverConfigViewModel);

            await _serverConfigWriter.WriteConfig(serverConfig);

            //return View();
        }

        public async Task<IActionResult> SaveConfig()
        {
            // map view to model 
            
            return View();
        }

        private ServerConfig MapServerConfigViewModel(ServerConfigViewModel serverConfigViewModel)
        {
            var serverConfig = new ServerConfig();
            serverConfig.Defaults = serverConfigViewModel.Defaults;
            foreach (var server in serverConfigViewModel.Servers)
            {
                serverConfig.Servers.Add(new Server
                {
                    Name = server.Name,
                    Overrides = server.Overrides
                });
            }

            return serverConfig;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}