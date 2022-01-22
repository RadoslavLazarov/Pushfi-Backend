using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Domain.Configuration;
using RestSharp;

namespace Pushfi.Infrastructure.Services
{
	public class SoapService : ISoapService
	{
		private readonly EnfortraConfiguration _enfortraConfiguration;

		public SoapService(IOptionsMonitor<EnfortraConfiguration> optionsMonitor)
		{
			this._enfortraConfiguration = optionsMonitor.CurrentValue;
		}

		public async Task<T> RequestAsync<T>(string requestBody)
		{
			var client = new RestClient(this._enfortraConfiguration.ApiUrl);
			client.Timeout = -1;

			var request = new RestRequest(Method.POST);
			request.AddHeader("Content-Type", "text/xml;charset=UTF-8");
			request.AddParameter("text/xml", requestBody, ParameterType.RequestBody);

			var response = await client.ExecuteAsync<T>(request);
			return response.Data;
		}
	}
}
