using System;
using System.ComponentModel.DataAnnotations;

namespace Pushfi.Application.Common.Models.Authentication
{
	public class LoginRequestModel
	{
		// TODO: move messages in different file
		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }
	}
}
