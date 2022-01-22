using System.Threading.Tasks;

namespace Pushfi.Application.Common.Interfaces
{
	public interface ISoapService
	{
		Task<T> RequestAsync<T>(string requestBody);
	}
}
