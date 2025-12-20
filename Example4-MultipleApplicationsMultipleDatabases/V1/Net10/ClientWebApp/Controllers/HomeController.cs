using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks.Cache;
using ServiceBricks.Logging;
using ServiceBricks.Notification;
using ServiceBricks.Security;
using ServiceBricks.Work;
using ServiceQuery;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ICacheDataApiClient _cacheDataApiClient;
        private readonly ILogMessageApiClient _logMessageApiClient;
        private readonly INotifyMessageApiClient _notifyMessageApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly IUserAuditApiClient _userAuditApiClient;
        private readonly IProcessApiClient _processApiClient;


        public HomeController(
            ICacheDataApiClient cacheDataApiClient,
            ILogMessageApiClient logMessageApiClient,
            INotifyMessageApiClient notifyMessageApiClient,
            IUserApiClient userApiClient,
            IUserAuditApiClient userAuditApiClient,
            IProcessApiClient processApiClient)
        {
            _cacheDataApiClient = cacheDataApiClient;
            _logMessageApiClient = logMessageApiClient;
            _notifyMessageApiClient = notifyMessageApiClient;
            _userApiClient = userApiClient;
            _userAuditApiClient = userAuditApiClient;
            _processApiClient = processApiClient;
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
        public IActionResult ViewServiceData(string create)
        {
            ViewServiceDataViewModel model = new ViewServiceDataViewModel();

            // Create new example record if requested
            if (!string.IsNullOrEmpty(create))
                CreateAnExampleRecord(create);

            // Create default query, 1st page of 1000 records (no count)
            var defaultQuery = new ServiceQueryRequestBuilder().Build();

            // Query logging microservice
            var respQueryLogMessages = _logMessageApiClient.Query(defaultQuery);
            model.LogMessages = respQueryLogMessages.Item.List;

            // Query security microservice
            var respQueryApplicationUsers = _userApiClient.Query(defaultQuery);
            model.Users = respQueryApplicationUsers.Item.List;
            var respQueryApplicationUserAudits = _userAuditApiClient.Query(defaultQuery);
            model.UserAudits = respQueryApplicationUserAudits.Item.List;

            // Query cache microservice
            var respQueryCacheData = _cacheDataApiClient.Query(defaultQuery);
            model.CacheDatas = respQueryCacheData.Item.List;

            // Query notification microservice
            var respQueryNotifyMessages = _notifyMessageApiClient.Query(defaultQuery);
            model.Notifications = respQueryNotifyMessages.Item.List;

            // Query notification microservice log messages
            _logMessageApiClient.BaseUrl = _notifyMessageApiClient.BaseUrl;
            var respNotificationLogMessages = _logMessageApiClient.Query(defaultQuery);
            model.NotificationLogMessages = respNotificationLogMessages.Item.List;

            // Query work microservice process messages            
            var respProcesses = _processApiClient.Query(defaultQuery);
            model.Processes = respProcesses.Item.List;

            return View("ViewServiceData", model);
        }

        private void CreateAnExampleRecord(string create)
        {
            switch (create)
            {
                default:
                case "logging":

                    // Create a logmessage
                    var newLogMessage = new LogMessageDto()
                    {
                        Application = "ClientWebApp",
                        Category = "Information",
                        Message = "This is a test message " + Guid.NewGuid().ToString(),
                    };
                    var respCreateLogMessage = _logMessageApiClient.Create(newLogMessage);

                    break;

                case "cache":

                    // Create a cachedata
                    var newCacheData = new CacheDataDto()
                    {
                        CacheKey = "ClientWebApp-" + Guid.NewGuid().ToString(),
                        CacheValue = "This is a test value " + Guid.NewGuid().ToString()
                    };
                    var respCreateCacheData = _cacheDataApiClient.Create(newCacheData);

                    break;

                case "notification":

                    // Create a new notifymessage
                    var newNotifyMessage = new NotifyMessageDto()
                    {
                        SenderType = SenderType.SMS_TEXT,
                        FromAddress = "1234567890",
                        ToAddress = "0987654321",
                        Body = "This is a test message " + Guid.NewGuid().ToString(),
                    };
                    var respCreateNotifyMessage = _notifyMessageApiClient.Create(newNotifyMessage);

                    break;

                case "security":

                    // Find a user
                    var respUsers = _userApiClient.Query(ServiceQueryRequestBuilder.New().Build());

                    // Create a useraudit
                    var newUserAudit = new UserAuditDto()
                    {
                        AuditType = "This is a test",
                        Data = "Test data",
                        IPAddress = ":1",
                        RequestHeaders = "test header info",
                        UserStorageKey = respUsers.Item.List[0].StorageKey
                    };
                    var respCreateUserAudit = _userAuditApiClient.Create(newUserAudit);

                    break;

                case "work":
                    

                    // Create a process
                    var newProcess = new ProcessDto()
                    {
                        ProcessData = "test data",
                        ProcessType = "test type",
                        ProcessQueue = "test queue"
                    };
                    var respCreateProcess = _processApiClient.Create(newProcess);

                    break;
            }
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