using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel.Home
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}