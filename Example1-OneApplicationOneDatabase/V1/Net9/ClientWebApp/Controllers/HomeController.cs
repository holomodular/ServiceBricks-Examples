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
        public async Task<IActionResult> ViewServiceData(string create)
        {
            ViewServiceDataViewModel model = new ViewServiceDataViewModel();

            // Create new example record if requested
            if (!string.IsNullOrEmpty(create))
                await CreateAnExampleRecordAsync(create);

            // Create default query
            var defaultQuery = new ServiceQueryRequestBuilder().Build();

            // Query logging microservice
            var respQueryLogMessages = await _logMessageApiClient.QueryAsync(defaultQuery);
            model.LogMessages = respQueryLogMessages.Item.List;

            // Query security microservice
            var respQueryApplicationUsers = await _userApiClient.QueryAsync(defaultQuery);
            model.Users = respQueryApplicationUsers.Item.List;
            var respQueryApplicationUserAudits = await _userAuditApiClient.QueryAsync(defaultQuery);
            model.UserAudits = respQueryApplicationUserAudits.Item.List;

            // Query cache microservice
            var respQueryCacheData = await _cacheDataApiClient.QueryAsync(defaultQuery);
            model.CacheDatas = respQueryCacheData.Item.List;

            // Query notification microservice
            var respQueryNotifyMessages = await _notifyMessageApiClient.QueryAsync(defaultQuery);
            model.Notifications = respQueryNotifyMessages.Item.List;

            var respQueryProcess = await _processApiClient.QueryAsync(defaultQuery);
            model.Processes = respQueryProcess.Item.List;

            return View("ViewServiceData", model);
        }

        private async Task CreateAnExampleRecordAsync(string create)
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
                    var respCreateLogMessage = await _logMessageApiClient.CreateAsync(newLogMessage);

                    break;

                case "cache":

                    // Create a cachedata
                    var newCacheData = new CacheDataDto()
                    {
                        CacheKey = "ClientWebApp-" + Guid.NewGuid().ToString(),
                        CacheValue = "This is a test value " + Guid.NewGuid().ToString()
                    };
                    var respCreateCacheData = await _cacheDataApiClient.CreateAsync(newCacheData);

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
                    var respCreateNotifyMessage = await _notifyMessageApiClient.CreateAsync(newNotifyMessage);

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
                    var respCreateUserAudit = await _userAuditApiClient.CreateAsync(newUserAudit);

                    break;
                case "work":

                    // Create a new process
                    var newProcess = new ProcessDto()
                    {                        
                        ProcessData = "test data",
                        ProcessQueue = "test queue",
                        ProcessType = "test type",
                        FutureProcessDate = DateTimeOffset.UtcNow,
                    };
                    var respProcess = await _processApiClient.CreateAsync(newProcess);

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