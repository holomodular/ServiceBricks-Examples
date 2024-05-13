using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks;
using ServiceBricks.Logging;
using ServiceBricks.Cache;
using ServiceBricks.Notification;
using ServiceBricks.Security;
using WebApp.ViewModel.Home;
using ServiceQuery;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogMessageApiClient _logMessageApiClient;
        private readonly IApplicationUserApiClient _applicationUserApiClient;
        private readonly INotifyMessageApiClient _notifyMessageApiClient;
        private readonly ICacheDataApiClient _cacheDataApiClient;

        public HomeController(
            ILogMessageApiClient logMessageApiClient,
            IApplicationUserApiClient applicationUserApiClient,
            INotifyMessageApiClient notifyMessageApiClient,
            ICacheDataApiClient cacheDataApiClient
            )
        {
            _logMessageApiClient = logMessageApiClient;
            _applicationUserApiClient = applicationUserApiClient;
            _notifyMessageApiClient = notifyMessageApiClient;
            _cacheDataApiClient = cacheDataApiClient;
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
        [Route("ViewServiceData")]
        public IActionResult ViewServiceData()
        {
            ViewServiceDataViewModel model = new ViewServiceDataViewModel();

            // Create a query to get all records
            var queryAll = new ServiceQueryRequestBuilder().Build();

            // Create a log message (logging microservice)
            var respCreateLogMessage = _logMessageApiClient.Create(new LogMessageDto()
            {
                Application = "ClientWebApp",
                Category = "Information",
                Message = "This is a test message " + Guid.NewGuid().ToString(),
            });

            model.LogMessages = new List<LogMessageDto>();

            // Query logging data (logging microservice)
            var respQueryLogMessages = _logMessageApiClient.Query(queryAll);
            if (respQueryLogMessages.Success && respQueryLogMessages.Item != null)
                model.LogMessages.AddRange(respQueryLogMessages.Item.List);

            // Query application user data
            var respQueryApplicationUsers = _applicationUserApiClient.Query(queryAll);
            if (respQueryApplicationUsers.Success && respQueryApplicationUsers.Item != null)
                model.Users = respQueryApplicationUsers.Item.List;

            // Create a cache data
            var newCacheData = new CacheDataDto()
            {
                Key = "ClientWebApp-" + Guid.NewGuid().ToString(),
                Value = "This is a test value " + Guid.NewGuid().ToString()
            };
            var respCreateCacheData = _cacheDataApiClient.Create(newCacheData);

            // Query cache data
            var respQueryCacheData = _cacheDataApiClient.Query(queryAll);
            if (respQueryCacheData.Success && respQueryCacheData.Item != null)
                model.CacheDatas = respQueryCacheData.Item.List;

            // Create a new notification (SMS just logs by default instead of sending)
            var newNotifyMessage = new NotifyMessageDto()
            {
                SenderTypeKey = SenderType.SMS,
                FromAddress = "1234567890",
                ToAddress = "0987654321",
                Body = "This is a test message " + Guid.NewGuid().ToString(),
            };
            var respCreateNotifyMessage = _notifyMessageApiClient.Create(newNotifyMessage);

            // Query notifications
            var respQueryNotifyMessages = _notifyMessageApiClient.Query(queryAll);
            if (respQueryNotifyMessages.Success && respQueryNotifyMessages.Item != null)
                model.Notifications = respQueryNotifyMessages.Item.List;

            return View("ViewServiceData", model);
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