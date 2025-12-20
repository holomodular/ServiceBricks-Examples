using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [Authorize]
    [Route("Account")]
    public class AccountController : Controller
    {
        [Authorize]
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect("/Login?returnUrl=" + returnUrl);
            return LocalRedirect("/Login");
        }
    }
}