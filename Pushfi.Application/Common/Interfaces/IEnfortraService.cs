using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Enfortra;
using Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport.GetFullCreditReportJson;

namespace Pushfi.Application.Common.Interfaces
{
	public interface IEnfortraService
	{
		Task<long> CreateNewUserEnrollmentAsync(CreateNewUserEnrollmentModel model);

		Task<CreditReportUrlModel> GetCreditReportDetailsAsync(string email);

		Task<bool> GetKBAStatusAsync(string email);

		Task<GetFullCreditReportJsonModel> GetFullCreditReportAsync(string email);

		Task<bool> CancelEnrollmentAsync(string email);
	}
}
