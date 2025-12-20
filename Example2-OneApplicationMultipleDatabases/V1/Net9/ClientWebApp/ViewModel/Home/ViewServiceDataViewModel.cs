using ServiceBricks.Cache;
using ServiceBricks.Logging;
using ServiceBricks.Notification;
using ServiceBricks.Security;
using ServiceBricks.Work;

namespace WebApp.ViewModel.Home
{
    public class ViewServiceDataViewModel
    {
        public List<UserDto> Users { get; set; }
        public List<UserAuditDto> UserAudits { get; set; }
        public List<LogMessageDto> LogMessages { get; set; }
        public List<CacheDataDto> CacheDatas { get; set; }
        public List<NotifyMessageDto> Notifications { get; set; }
        public List<ProcessDto> Processes { get; set; }
    }
}