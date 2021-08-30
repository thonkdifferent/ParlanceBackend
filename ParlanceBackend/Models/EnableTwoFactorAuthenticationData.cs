namespace ParlanceBackend.Models
{
    public class EnableTwoFactorAuthenticationData
    {
        public string CurrentPassword { get; set; }
        public string OtpCode { get; set; }
    }
}