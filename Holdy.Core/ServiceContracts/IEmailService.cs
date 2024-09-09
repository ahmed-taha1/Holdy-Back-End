namespace Holdy.Holdy.Core.ServiceContracts
{
    public interface IEmailService
    {
        public Task<bool> SendOTP(string email);
        public Task<bool> VerifyOTP(string email, int otp);
        public bool ReportBug(string message, string userEmail);
    }
}