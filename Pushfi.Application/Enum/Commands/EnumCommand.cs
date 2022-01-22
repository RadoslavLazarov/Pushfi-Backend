using MediatR;
using Pushfi.Domain.Extensions;

namespace Pushfi.Application.Enum.Commands
{
	public class EnumCommand : IRequest<List<EnumNameValue>>
	{
	}
}
