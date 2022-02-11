using Newtonsoft.Json;
using Pushfi.Common.Constants.User;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Pushfi.Application.Common.Models.Authentication
{
	public class CustomerModel
	{
		#region ApplicationUser
		public string UserId { get; set; }

		public Guid BrokerId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Email { get; set; }

		[Required]
		[MaxLength(100)]
		public string Password { get; set; }

		public bool IsDeleted { get; set; }
        #endregion

        public ProcessStatus ProcessStatus { get; set; }

		#region Customer private data
		public long EnfortraUserID { get; set; }

		[MaxLength(100)]
		public string Affiliate { get; set; }

		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string FirstName { get; set; }

		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string MiddleName { get; set; }

		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string LastName { get; set; }

		[MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
		public string PhoneNumber { get; set; }

		[Required]
		[MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
		public string MobilePhoneNumber { get; set; }

		[Required]
		[MaxLength(50)]
		public string SMSphoneCarrier { get; set; }

		[Required]
		[MaxLength(100)]
		public string StreetAddress { get; set; }

		[Required]
		[MaxLength(50)]
		public string City { get; set; }

		[Required]
		[MaxLength(50)]
		public string Region { get; set; }

		[Required]
		[MaxLength(10)]
		public string PostalCode { get; set; }

		[Required]
		[MaxLength(50)]
		public string HousingStatus { get; set; }

		[Required]
		public double MonthlyHousingPayment { get; set; }

		[Required]
		public DateTimeOffset DateMovedInThisAddress { get; set; }

		[Required]
		public DateTimeOffset DateOfBirth { get; set; }

		[Required]
		[MaxLength(9)]
		public string SSN { get; set; }

		[Required]
		[MaxLength(20)]
		public string MaritalStatus { get; set; }

		[Required]
		[MaxLength(100)]
		public string ESignature { get; set; }
		public bool? AgreementCommunications { get; set; }
		#endregion

		#region Loan data

		[Required]
		public double FundingAmountRequested { get; set; }

		public double? CurrentCreditScores { get; set; }

		[Required]
		public List<string> LoanProducts { get; set; }

		[Required]
		[MaxLength(10000)]
		public string DetailedUseOfFunds { get; set; }
		#endregion

		#region College/Millitary data

		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string CollegeUniversityName { get; set; }

		[MaxLength(50)]
		public string DegreeObtained { get; set; }

		[MaxLength(50)]
		public string CourceOfStudy { get; set; }

		[MaxLength(4)]
		public string YearGraduated { get; set; }

		[MaxLength(50)]
		public string CurrentMillitaryAffiliation { get; set; }

		[Required]
		[MaxLength(1000)]
		public string PresentEmployer { get; set; }

		[MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
		public string EmployerPhoneNumber { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Position { get; set; }

		[Required]
		public DateTimeOffset StartDateWithEmployer { get; set; }

		[Required]
		public double MonthlyGrossIncomeAmount { get; set; }

		[Required]
		public double TotalAnnualHouseholdIncome { get; set; }

		[Required]
		public double RetirementAccountBalance { get; set; }
		#endregion

		#region Business data
		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string CompanyName { get; set; }

		[MaxLength(UserEntityConstants.NameMaxLength)]
		public string DBAname { get; set; }

		[MaxLength(100)]
		public string BusinessAddress { get; set; }

		[MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
		public string BusinessPhoneNumber { get; set; }
		public DateTimeOffset? BusinessStartDate { get; set; }
		public double? PercentageOfOwnership { get; set; }

		[MaxLength(UserEntityConstants.TaxMaxLength)]
		public string TAXID { get; set; }

		[MaxLength(50)]
		public string CorpStructure { get; set; }

		public double? GrossAnnualRevenue { get; set; }

		public double? NetProfit { get; set; }

		public double? MonthlyLeaseOrCommercialLoanPayment { get; set; }

		[MaxLength(50)]
		public string BusinessLocationLeaseMortgage { get; set; }
		public double? BusinessLocationMonthlyPayment { get; set; }

		[MaxLength(20)]
		public string NumberOfEmployees { get; set; }

		[MaxLength(UserEntityConstants.WebsiteURLMaxLenght)]
		public string WebsiteURL { get; set; }
		#endregion
	}
}
