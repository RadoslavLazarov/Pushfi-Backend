using Newtonsoft.Json;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Models.Authentication
{
	public class CustomerModel
	{
		public string UserId { get; set; }
        public bool IsDeleted { get; set; }
		public ProcessStatus ProcessStatus { get; set; }

		#region Customer private data
		public long EnfortraUserID { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Affiliate { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string MobilePhoneNumber { get; set; }
		public string SMSphoneCarrier { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string HousingStatus { get; set; }
		public double MonthlyHousingPayment { get; set; }
		public DateTimeOffset DateMovedInThisAddress { get; set; }
		public DateTimeOffset DateOfBirth { get; set; }
		public string SSN { get; set; }
		public string MaritalStatus { get; set; }
		public string ESignature { get; set; }
		public bool? AgreementCommunications { get; set; }
        #endregion

        #region Loan data
        public double FundingAmountRequested { get; set; }
		public double? CurrentCreditScores { get; set; }
		//public LoanProductsModel LoanProducts { get; set; }
		public List<string> LoanProducts { get; set; }
		public string DetailedUseOfFunds { get; set; }
		#endregion

		#region College/Millitary data
		public string CollegeUniversityName { get; set; }
		public string DegreeObrained { get; set; }
		public string CourceOfStudy { get; set; }
		public string YearGraduated { get; set; }
		public string CurrentMillitaryAffiliation { get; set; }
		public string PresentEmployer { get; set; }
		public string EmployerPhoneNumber { get; set; }
		public string Position { get; set; }
		public DateTimeOffset StartDateWithEmployer { get; set; }
		public double MonthlyGrossIncomeAmount { get; set; }
        public double TotalAnnualHouseholdIncome { get; set; }
		public double RetirementAccountBalance { get; set; }
		#endregion

		#region Business data
		public string CompanyName { get; set; }
		public string DBAname { get; set; }
		public string BusinessAddress { get; set; }
		public string BusinessPhoneNumber { get; set; }
		public DateTimeOffset? BusinessStartDate { get; set; }
		public double? PercentageOfOwnership { get; set; }
		public string TAXID { get; set; }
		public string CorpStructure { get; set; }
		public double? GrossAnnualRevenue { get; set; }
		public double? NetProfit { get; set; }
		public double? MonthlyLeaseOrCommercialLoanPayment { get; set; }
		public string BusinessLocationLeaseMortgage { get; set; }
		public double? BusinessLocationMonthlyPayment { get; set; }
		public string NumberOfEmployees { get; set; }
		public string WebsiteURL { get; set; }
		#endregion
	}
}
