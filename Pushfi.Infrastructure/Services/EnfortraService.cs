using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Enfortra;
using Pushfi.Application.Common.Models.Enfortra.Response;
using Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport;
using Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport.GetFullCreditReportJson;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Exceptions;
using Pushfi.Infrastructure.Enums;
using System.Text;

namespace Pushfi.Infrastructure.Services
{
	public class EnfortraService : IEnfortraService
	{
		private readonly EnfortraConfiguration _enfortraConfiguration;
		private readonly ISoapService _soapService;

		public EnfortraService(
			IOptionsMonitor<EnfortraConfiguration> optionsMonitor,
			ISoapService soapService)
		{
			this._enfortraConfiguration = optionsMonitor.CurrentValue;
			this._soapService = soapService;
		}

		public async Task<long> CreateNewUserEnrollmentAsync(CreateNewUserEnrollmentModel model)
		{
			StringBuilder xml = new StringBuilder();
			xml.Append(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
			xml.Append(@"<soap:Body>");
			xml.Append(@"<CreateNewUserEnrollment xmlns=""https://api.identityprotection-services.com"">");
			xml.Append(@"<APILoginName>" + this._enfortraConfiguration.LoginName + "</APILoginName>");
			xml.Append(@"<APILoginPassword>" + this._enfortraConfiguration.LoginPassword + "</APILoginPassword>");
			xml.Append(@"<PartnerID>" + this._enfortraConfiguration.PartnerID + "</PartnerID>");
			xml.Append(@"<MemberID>" + this._enfortraConfiguration.MemberID + "</MemberID>");
			xml.Append(@"<UserEmailAddress>" + model.Email + "</UserEmailAddress>");
			xml.Append(@"<UserPassword>" + model.Password + "</UserPassword>");
			xml.Append(@"<SSN>" + model.SSN + "</SSN>");
			xml.Append(@"<DOB>" + model.DOB.ToString("yyyy-MM-dd") + "</DOB>");
			xml.Append(@"<FirstName>" + model.FirstName + "</FirstName>");
			xml.Append(@"<MiddleName></MiddleName>");
			xml.Append(@"<LastName>" + model.LastName + "</LastName>");
			xml.Append(@"<Suffix></Suffix>");
			xml.Append(@"<Address1>" + model.Address + "</Address1>");
			xml.Append(@"<Address2></Address2>");
			xml.Append(@"<City>" + model.City + "</City>");
			xml.Append(@"<State>" + model.State + "</State>");
			xml.Append(@"<Zipcode>" + model.ZipCode + "</Zipcode>");
			xml.Append(@"<CountryCode>US</CountryCode>");
			xml.Append(@"<LanguageCode>EN</LanguageCode>");
			xml.Append(@"<PhoneNumber>" + model.SMSPhone + "</PhoneNumber>");
			xml.Append(@"<PhoneType>Home</PhoneType>");
			xml.Append(@"<SMSNumber>" + model.SMSPhone + "</SMSNumber>");
			xml.Append(@"<SMSCarrier>" + model.SMSPhoneCarrier + "</SMSCarrier>");
			xml.Append(@"<BundleID>" + this._enfortraConfiguration.BundleID + "</BundleID>");
			xml.Append(@"<GenerateToken>No</GenerateToken>");
			xml.Append(@"<ErrMsg></ErrMsg>");
			xml.Append(@"</CreateNewUserEnrollment>");
			xml.Append(@"</soap:Body>");
			xml.Append(@"</soap:Envelope>");

			var requestBody = xml.ToString();
			var response = await this._soapService.RequestAsync<CreateNewUserEnrollmentResponse>(requestBody);

			if (!string.IsNullOrEmpty(response.ErrMsg))
			{
				throw new BusinessException(response.ErrMsg);
			}

			return response.CreateNewUserEnrollmentResult;
		}

		public async Task<CreditReportUrlModel> GetCreditReportDetailsAsync(string email)
		{
			var response = await EnfortraRequestAsync
				<GetCreditReportDetailsResponse>(EnfortraRequestType.GetCreditReportDetails, email);

			if (!string.IsNullOrEmpty(response.ErrMsg))
			{
				throw new BusinessException(response.ErrMsg);
			}

			var creditReportURL = response.GetCreditReportDetailsResult;

			return new CreditReportUrlModel() { CreditReportUrl = creditReportURL };
		}

        public async Task<bool> GetKBAStatusAsync(string email)
		{
			var response = await EnfortraRequestAsync
				<GetKBAStatusResponse>(EnfortraRequestType.GetKBAStatus, email);

			if (!string.IsNullOrEmpty(response.ErrMsg))
			{
				throw new BusinessException(response.ErrMsg);
			}

			return response.GetKBAStatusResult;
		}

		public async Task<GetFullCreditReportJsonModel> GetFullCreditReportAsync(string email)
		{
			var response = await EnfortraRequestAsync
				<GetFullCreditReportResponse>(EnfortraRequestType.GetFullCreditReport, email);


            if (!string.IsNullOrEmpty(response.ErrMsg))
            {
                throw new BusinessException(response.ErrMsg);
            }

            var result = JsonConvert.DeserializeObject<GetFullCreditReportJsonModel>(response.GetFullCreditReportResult);
			return result;
		}

		public async Task<bool> CancelEnrollmentAsync(string email)
		{
			var response = await EnfortraRequestAsync
				<CancelEnrollmentResponse>(EnfortraRequestType.CancelEnrollment, email);

			if (!string.IsNullOrEmpty(response.ErrMsg))
			{
				throw new BusinessException(response.ErrMsg);
			}

			return true;
		}

		private async Task<T> EnfortraRequestAsync<T>(EnfortraRequestType requestType, string email)
        {
			var type = requestType.ToString();

			StringBuilder xml = new StringBuilder();
			xml.Append(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
			xml.Append(@"<soap:Body>");
			xml.Append(@"<" + type + @" xmlns=""https://api.identityprotection-services.com"">");
			xml.Append(@"<APILoginName>" + this._enfortraConfiguration.LoginName + "</APILoginName>");
			xml.Append(@"<APILoginPassword>" + this._enfortraConfiguration.LoginPassword + "</APILoginPassword>");
			xml.Append(@"<UserEmailAddress>" + email + "</UserEmailAddress>");
			xml.Append(@"<OutputType>JSON</OutputType>");
			xml.Append(@"<ErrMsg></ErrMsg>");
			xml.Append(@"</" + type + ">");
			xml.Append(@"</soap:Body>");
			xml.Append(@"</soap:Envelope>");

			var requestBody = xml.ToString();
			var result = await this._soapService.RequestAsync<T>(requestBody);

			return result;
		}
	}
}
