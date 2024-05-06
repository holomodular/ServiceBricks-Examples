using ServiceBricks.Logging;
using ServiceBricks.Cache;
using ServiceBricks.Notification;
using ServiceBricks.Security;

namespace WebApp.ViewModel.Home
{
    public class ViewServiceDataViewModel
    {
        public List<ApplicationUserDto> Users { get; set; }
        public List<LogMessageDto> LogMessages { get; set; }
        public List<CacheDataDto> CacheDatas { get; set; }
        public List<NotifyMessageDto> Notifications { get; set; }
    }
}