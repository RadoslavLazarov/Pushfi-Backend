using Pushfi.Common.Interfaces;

namespace Pushfi.Application.Common.Models
{
	public class TrackableModelBase : ModelBase, ITrackable
	{
		public DateTimeOffset CreatedAt { get; set; }
		public Guid CreatedById { get; set; }
		public DateTimeOffset? ModifiedAt { get; set; }
		public Guid? ModifiedById { get; set; }
	}
}
