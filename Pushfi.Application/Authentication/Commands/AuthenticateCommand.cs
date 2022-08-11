using MediatR;
using Pushfi.Application.Common.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Pushfi.Application.Authentication.Commands
{
    public class AuthenticateCommand : IRequest<AuthenticateResponseModel>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string IpAddress { get; set; }
    }
}
