using Holdy.Holdy.Core.DTO;
using Holdy.Holdy.Core.ServiceContracts;
using Holdy.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Holdy.Holdy.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public MailController(IEmailService emailService, IJwtService jwtService)
        {
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult ReportBug([FromBody] DtoReportBugRequest request)
        {
            string token = HttpContext.Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;
            if (_emailService.ReportBug(request.Message, userEmail))
            {
                return Ok();
            }

            return BadRequest(
                new DtoErrorsResponse
                {
                    errors = new List<string> { "Bug reporting failed" }
                });
        }
    }
}