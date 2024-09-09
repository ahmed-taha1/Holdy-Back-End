using System.Net;
using System.Net.Mail;
using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.Domain.UnitOfWorkContracts;
using Holdy.Holdy.Core.ServiceContracts;

namespace Holdy.Holdy.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public EmailService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<bool> SendOTP(string email)
        {
            try
            {
                OTP otp = new OTP
                {
                    UserEmail = email,
                };
                var smtpServer = _configuration["SmtpSettings:Server"];
                int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]!);
                string? smtpUsername = _configuration["SmtpSettings:Username"];
                var smtpPassword = _configuration["SmtpSettings:Password"];
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    var fromAddress = new MailAddress(smtpUsername!, "Holdy");
                    var toAddress = new MailAddress(email);

                    var mailMessage = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "Your OTP",
                        Body = $"Your OTP is: {otp.OTPCode}",
                        IsBodyHtml = false
                    };

                    client.Send(mailMessage);
                }
                await RemoveOldOTPIfExist(email);
                await _unitOfWork.OTPs.InsertAsync(otp);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task RemoveOldOTPIfExist(string email)
        {
            OTP? oldOTP = await _unitOfWork.OTPs.SelectByMatchAsync(x => x.UserEmail == email);
            if (oldOTP != null)
            {
                await _unitOfWork.OTPs.DeleteAsync(oldOTP);
            }
        }

        public async Task<bool> VerifyOTP(string email, int otpCode)
        {
            OTP? otp = await _unitOfWork.OTPs.SelectByMatchAsync(x => x.UserEmail == email);
            if (otp == null || otp.OTPCode != otpCode || otp.ExpirationDate < DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public bool  ReportBug(string message, string userEmail)
        {
            const string receiver = "smtpserviceprotection@gmail.com";
            try {
                string? smtpServer = _configuration["SmtpSettings:Server"];
                int smtpPort = int.Parse(_configuration["SmtpSettings:Port"]!);
                string? smtpUsername = _configuration["SmtpSettings:Username"];
                string? smtpPassword = _configuration["SmtpSettings:Password"];
                using (var client = new SmtpClient(smtpServer, smtpPort)) {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;
                    var fromAddress = new MailAddress(smtpUsername!, "Holdy");
                    var toAddress = new MailAddress(receiver);

                    var mailMessage = new MailMessage(fromAddress, toAddress) {
                        Subject = "Bug Report",
                        Body = $"bug report sent from: {userEmail}\n" + message,
                        IsBodyHtml = false
                    };

                    client.Send(mailMessage);
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
