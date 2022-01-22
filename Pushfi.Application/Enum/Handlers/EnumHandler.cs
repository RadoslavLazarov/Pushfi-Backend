using MediatR;
using Pushfi.Application.Enum.Commands;
using Pushfi.Domain.Extensions;

namespace Pushfi.Application.Enum.Handlers
{
    public class EnumHandler : IRequestHandler<EnumCommand, List<EnumNameValue>>
    {
        public Task<List<EnumNameValue>> Handle(EnumCommand request, CancellationToken cancellationToken)
        {
            var enumsAsJson = EnumExtensions.AllEnumsAsJson();
            return Task.FromResult(enumsAsJson);
        }
    }
}
