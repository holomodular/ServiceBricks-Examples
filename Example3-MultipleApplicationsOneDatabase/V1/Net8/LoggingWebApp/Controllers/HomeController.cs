using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IServiceBus _serviceBus;

        public HomeController(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            return View(model);
        }

        [HttpGet]
        [Route("SendServiceBusMessage")]
        public IActionResult SendServiceBusMessage()
        {
            // Send ServiceBus Message
            CreateApplicationEmailBroadcast broadcast = new CreateApplicationEmailBroadcast(new ApplicationEmailDto()
            {
                Body = "Body From Logging Service",
                Subject = "Subject From Logging Service",
                ToAddress = "test@servicebricks.com",
            });
            _serviceBus.Send(broadcast);

            HomeViewModel model = new HomeViewModel();
            return View("Index", model);
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string message = null)
        {
            var model = new ErrorViewModel()
            {
                Message = message
            };
            return View("Error", model);
        }
    }
}