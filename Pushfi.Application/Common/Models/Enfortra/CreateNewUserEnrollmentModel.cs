namespace Pushfi.Application.Common.Models.Enfortra
{
	public class CreateNewUserEnrollmentModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string SSN { get; set; }
		public DateTimeOffset DOB { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string SMSPhone { get; set; }
		public string SMSPhoneCarrier { get; set; }
	}
}
