using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks.Security;
using ServiceBricks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IBusinessRuleService _businessRuleService;

        public HomeController(
            IUserManagerService userManagerService,
            IBusinessRuleService businessRuleService)
        {
            _userManagerService = userManagerService;
            _businessRuleService = businessRuleService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string message = null)
        {
            return View("Error", new ErrorViewModel() { Message = message });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Logout")]
        public virtual async Task<IActionResult> Logout()
        {
            return await LogOff();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Logoff")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> LogOff()
        {
            var userId = User?.GetId();
            if (!string.IsNullOrEmpty(userId))
            {
                // Call logout process
                ServiceBricks.Security.UserLogoutProcess process = new ServiceBricks.Security.UserLogoutProcess(userId);
                var respE = await _businessRuleService.ExecuteProcessAsync(process);
            }
            return RedirectToLocal();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public virtual async Task<IActionResult> Login(string returnUrl = null)
        {
            await EnsureLoggedOut();
            ModelState.Clear();
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Start login process
            UserLoginProcess process = new UserLoginProcess(
                model.Email,
                model.Password,
                model.RememberMe);
            var respLogin = await _businessRuleService.ExecuteProcessAsync(process);

            if (respLogin.Success)
            {
                // Login success
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToLocal(@"/Account");
                else
                    return RedirectToLocal(returnUrl);
            }
            else
            {
                // Login error
                if (process.ApplicationSigninResult != null)
                {
                    if (process.ApplicationSigninResult.EmailNotConfirmed)
                        return View("RegisterComplete");
                    if (process.ApplicationSigninResult.SignInResult != null)
                    {
                        if (process.ApplicationSigninResult.SignInResult.RequiresTwoFactor)
                            return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, model.RememberMe });
                        if (process.ApplicationSigninResult.SignInResult.IsLockedOut)
                            return View("Lockout");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(model);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Register")]
        public virtual async Task<IActionResult> Register()
        {
            await EnsureLoggedOut();
            return View(new RegisterViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            // Start registration process
            UserRegisterProcess process = new UserRegisterProcess(
                model.Email.Trim().ToLower(),
                model.Password.Trim());
            var respEvent = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (respEvent.Error)
            {
                ModelState.CopyFromResponse(respEvent);
                return View(model);
            }
            return View("RegisterComplete");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RegisterResendEmail")]
        public virtual ActionResult RegisterResendEmail()
        {
            return View(nameof(RegisterResendEmail), new ForgotPasswordViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterResendEmail")]
        public virtual async Task<ActionResult> RegisterResendEmail(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var respU = await _userManagerService.FindByEmailAsync(model.Email.Trim());
            if (respU.Item == null)
                return View("AccountNotFound");

            // Start process
            UserResendConfirmationProcess process = new UserResendConfirmationProcess(respU.Item.StorageKey);
            var respE = await _businessRuleService.ExecuteProcessAsync(process);
            if (respE.Error)
            {
                ModelState.CopyFromResponse(respE);
                return View(model);
            }

            return View("RegisterComplete");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public virtual async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");

            // Start process
            UserConfirmEmailProcess process = new UserConfirmEmailProcess(userId, code);
            var resp = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (resp.Error)
            {
                ModelState.CopyFromResponse(resp);
                var responseErr = new Response();
                responseErr.CopyFrom(resp);
                return View(nameof(ConfirmEmail), responseErr);
            }

            // Add any messages
            var response = new Response();
            response.CopyFrom(resp);
            return View(nameof(ConfirmEmail), response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPassword")]
        public virtual async Task<IActionResult> ForgotPassword()
        {
            await EnsureLoggedOut();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var respU = await _userManagerService.FindByEmailAsync(model.Email);
            if (respU.Item == null)
                return View("AccountNotFound");

            if (!respU.Item.EmailConfirmed)
                return View(nameof(ForgotPasswordConfirmation), new Response());

            // Start process
            UserForgotPasswordProcess process = new UserForgotPasswordProcess(respU.Item.StorageKey);
            var respProcess = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (respProcess.Error)
            {
                ModelState.CopyFromResponse(respProcess);
                return View(model);
            }
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ForgotPasswordConfirmation")]
        public virtual IActionResult ForgotPasswordConfirmation()
        {
            return View(new Response());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPassword")]
        public virtual IActionResult ResetPassword(string code = null)
        {
            if (string.IsNullOrEmpty(code))
                return View("Error");
            return View(new ResetPasswordViewModel() { Code = code });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Start process
            UserPasswordResetProcess process = new UserPasswordResetProcess(
                model.Email,
                model.Password,
                model.Code.Replace(" ", "+"));
            var resp = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (resp.Error)
            {
                ModelState.CopyFromResponse(resp);
                return View(model);
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPasswordConfirmation")]
        public virtual IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("SendCode")]
        public virtual async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var respUser = await _userManagerService.GetTwoFactorAuthenticationUserAsync();
            if (respUser.Item == null)
                return View("Error");

            var respProviders = await _userManagerService.GetValidTwoFactorProvidersAsync(
                respUser.Item.StorageKey);
            var factorOptions = respProviders.List.Select(p => new SelectListItem
            {
                Text = p,
                Value = p
            }).ToList();
            return View(new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SendCode")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            // Start process
            UserMfaProcess process = new UserMfaProcess(
                model.SelectedProvider);
            var respProcess = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (respProcess.Error)
            {
                ModelState.CopyFromResponse(respProcess);
                return View("Error");
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VerifyCode")]
        public virtual ActionResult VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("VerifyCode")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Start process
            UserMfaVerifyProcess process = new UserMfaVerifyProcess(
                model.Provider,
                model.Code,
                model.RememberMe,
                model.RememberBrowser);
            var resp = await _businessRuleService.ExecuteProcessAsync(process);

            // Check if error
            if (resp.Error)
            {
                if (process.SignInResult != null)
                {
                    if (process.SignInResult.IsLockedOut)
                        return View("Lockout");
                }
                ModelState.CopyFromResponse(resp);
                return View(model);
            }

            if (string.IsNullOrEmpty(model.ReturnUrl))
                return LocalRedirect("/Account");
            return LocalRedirect(model.ReturnUrl);
        }

        protected virtual void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected virtual IActionResult RedirectToLocal(string returnUrl = null)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return Redirect(@"/");
        }

        protected virtual async Task EnsureLoggedOut()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                await LogOff();
        }
    }
}