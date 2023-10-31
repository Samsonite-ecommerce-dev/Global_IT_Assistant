namespace Samsonite.Library.Web.Core.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string VerificationCode { get; set; }

        public bool IsRemember { get; set; }
    }

    public class ForgetPasswordRequest
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
