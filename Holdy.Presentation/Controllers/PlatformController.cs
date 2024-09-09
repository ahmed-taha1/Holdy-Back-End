﻿using Holdy.Holdy.Core.Domain.Entities;
using Holdy.Holdy.Core.DTO;
using Holdy.Holdy.Core.ServiceContracts;
using Holdy.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Holdy.Holdy.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly IJwtService _jwtService;
        public PlatformController(IPlatformService platformService, IJwtService jwtService)
        {
            _platformService = platformService;
            _jwtService = jwtService;
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePlatform([FromBody] DtoCreatePlatformRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;

            Platform platform = new Platform
            {
                PlatformName = request.PlatformName,
                IconColor = request.IconColor
            };

            if (await _platformService.CreatePlatformAsync(platform, userEmail))
            {
                DtoCreatePlatformResponse response = new DtoCreatePlatformResponse
                {
                    PlatformId = platform.Id,
                };
                return StatusCode(StatusCodes.Status201Created, response);
            }
            return BadRequest();
        }

        [HttpDelete]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> DeletePlatform([FromBody] DtoDeletePlatformRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            if (await _platformService.DeletePlatformAsync(request.Id, userId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlatforms()
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            ICollection<Platform?>? platforms = await _platformService.GetAllPlatforms(userId);
            DtoGetAllPlatformsResponse response = new DtoGetAllPlatformsResponse
            {
                Platforms = platforms!.Select(p => new DtoPlatform
                {
                    PlatformId = p!.Id,
                    PlatformName = p.PlatformName,
                    IconColor = p.IconColor,
                    NumOfAccounts = p.Accounts!.Count
                }).ToList()
            };
            return Ok(response);
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePlatform([FromBody] DtoUpdatePlatformRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userEmail = _jwtService.GetEmailFromToken(token)!;
            Platform platform = new Platform
            {
                Id = request.Id,
                PlatformName = request.PlatformName,
                IconColor = request.IconColor
            };

            if (await _platformService.UpdatePlatformAsync(platform, userEmail))
            {
                return Ok();
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "platform not found or error happened" } });
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetPlatform([FromBody] DtoGetPlatformRequest request)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            Platform? platform = await _platformService.GetPlatformByIdAsync(request.Id, userId);
            if (platform != null)
            {
                DtoPlatform response = new DtoPlatform()
                {
                    PlatformId = platform.Id,
                    PlatformName = platform.PlatformName,
                    IconColor = platform.IconColor,
                    NumOfAccounts = platform.Accounts!.Count,
                };
                return Ok(response);
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "platform not found" } });
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetPlatformWithAccounts([FromBody] DtoGetPlatformRequest request,
            [FromServices] IAccountService accountService)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            Platform? platform = await _platformService.GetPlatformByIdAsync(request.Id, userId);
            if (platform != null)
            {
                DtoPlatformWithAccounts response = new DtoPlatformWithAccounts
                {
                    PlatformId = platform.Id,
                    PlatformName = platform.PlatformName,
                    IconColor = platform.IconColor,
                    NumOfAccounts = platform.Accounts!.Count,
                };
                response.Accounts = await accountService.GetAccountsByPlatformIdAsync(platform.Id, userId);
                return Ok(response);
            }
            return BadRequest(new DtoErrorsResponse { errors = new List<string> { "platform not found" } });
        }


        [HttpGet]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAllPlatformsWithAccounts([FromServices] IAccountService accountService)
        {
            string token = Request.Headers["Authorization"]!;
            string userId = _jwtService.GetIdFromToken(token)!;
            ICollection<Platform?>? platforms = await _platformService.GetAllPlatforms(userId);
            if (platforms == null)
            {
                return BadRequest(new DtoErrorsResponse { errors = new List<string> { "no platforms found found" } });
            }
            DtoGetAllPlatformsWithAccountsResponse response = new DtoGetAllPlatformsWithAccountsResponse
            {
                Platforms = platforms.Select(p => new DtoPlatformWithAccounts
                {
                    PlatformId = p!.Id,
                    PlatformName = p.PlatformName,
                    IconColor = p.IconColor,
                    NumOfAccounts = p.Accounts!.Count,
                }).ToList()
            };

            foreach (DtoPlatformWithAccounts platform in response.Platforms)
            {
                platform.Accounts = await accountService.GetAccountsByPlatformIdAsync(platform.PlatformId, userId);
            }
            return Ok(response);
        }
    }
}