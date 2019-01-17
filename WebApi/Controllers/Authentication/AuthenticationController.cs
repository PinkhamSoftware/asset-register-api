using System.Threading;
using System.Threading.Tasks;
using HomesEngland.UseCase.AuthenticateUser;
using HomesEngland.UseCase.AuthenticateUser.Models;
using HomesEngland.UseCase.GetAccessToken;
using HomesEngland.UseCase.GetAccessToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers.Authentication
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateUser _authenticateUser;
        private readonly IGetAccessToken _getAccessToken;

        public AuthenticationController(IAuthenticateUser authenticateUser, IGetAccessToken getAccessToken)
        {
            _authenticateUser = authenticateUser;
            _getAccessToken = getAccessToken;
        }

        public class AuthenticationAuthoriseRequest
        {
            public string Url { get; set; }
            public string Email { get; set; }
        }

        public class AuthenticationAccessTokenRequest
        {
            public string Token { get; set; }
        }

        [HttpPost("authorise")]
        [Produces("application/json")]
        public async Task<IActionResult> AuthorisePost(
            [FromBody] AuthenticationAuthoriseRequest authenticationAuthoriseRequest)
        {
            if (authenticationAuthoriseRequest.Url == null || authenticationAuthoriseRequest.Email == null)
            {
                return StatusCode(400);
            }

            await _authenticateUser.ExecuteAsync(new AuthenticateUserRequest
            {
                Url = authenticationAuthoriseRequest.Url,
                Email = authenticationAuthoriseRequest.Email
            }, new CancellationToken());

            return StatusCode(200);
        }

        [HttpPost("access_token")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAccessTokenResponse), 200)]
        public async Task<IActionResult> AccessTokenPost(
            [FromBody] AuthenticationAccessTokenRequest authenticationAccessTokenRequest)
        {
            if (authenticationAccessTokenRequest.Token == null)
            {
                return StatusCode(400);
            }

            var response = await _getAccessToken.ExecuteAsync(new GetAccessTokenRequest
            {
                Token = authenticationAccessTokenRequest.Token
            }, new CancellationToken());

            var responseData = new ResponseData<GetAccessTokenApiResponse>(new GetAccessTokenApiResponse
                {AccessToken = response.AccessToken});

            return StatusCode(200, responseData);
        }

        [HttpGet("verification")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            return StatusCode(200);
        }

        public class GetAccessTokenApiResponse
        {
            public string AccessToken { get; set; }
        }
    }
}
