using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Authentication.Commands;
using Pushfi.Application.Broker.Commands;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.WebAPI.Controllers
{
    [Authorize]
    public class AuthenticationController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost(nameof(Authenticate))]
        public async Task<ActionResult<AuthenticateResponseModel>> Authenticate(AuthenticateCommand command)
        {
            command.IpAddress = this.IpAddress();

            var response = await Mediator.Send(command);

            this.SetTokenCookie(response.RefreshToken);

            return response;
        }

        [AllowAnonymous]
        [HttpPost(nameof(RefreshToken))]
        public async Task<ActionResult<AuthenticateResponseModel>> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            //command.RefreshToken = Request.Cookies["refreshToken"];
            command.IpAddress = this.IpAddress();

            var response = await Mediator.Send(command);

            this.SetTokenCookie(response.RefreshToken);

            return response;
        }

        [HttpPost(nameof(RevokeToken))]
        public async Task<ActionResult<RevokeTokenResponseModel>> RevokeToken(RevokeTokenCommand command)
        {    
            command.Token = command.Token ?? Request.Cookies["refreshToken"];
            command.IpAddress = this.IpAddress();

            return await Mediator.Send(command);
        }

        [HttpGet("{id}/" + nameof(GetRefreshTokens))]
        public async Task<ActionResult<GetRefreshTokensResponseModel>> GetRefreshTokens([FromRoute] GetRefreshTokensCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{BrokerPath}/" + nameof(CustomerRegistration))]
        public async Task<ActionResult<RegistrationResponseModel>> CustomerRegistration(RegistrationCommand command, [FromRoute] string brokerPath)
        {
            command.BrokerPath = brokerPath;
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("{BrokerPath}/" + nameof(CustomerAuthenticate))]
        public async Task<ActionResult<LoginResponseModel>> CustomerAuthenticate(LoginCommand command, [FromRoute] string brokerPath)
        {
            command.BrokerPath = brokerPath;
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(nameof(BrokerRegistration))]
        public async Task BrokerRegistration([FromForm] BrokerRegistrationCommand command)
        {
            await Mediator.Send(command);
        }

        [HttpGet]
        [Route(nameof(Me))]
        public async Task<ActionResult<MeResponseModel>> Me([FromHeader] MeCommand command)
        {
            return await Mediator.Send(command);
        }

        // helper methods
        private void SetTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
