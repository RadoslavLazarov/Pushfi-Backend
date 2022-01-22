﻿namespace Pushfi.Common.Interfaces
{
	public interface ITrackable
	{
		DateTimeOffset CreatedAt { get; set; }
		Guid CreatedById { get; set; }
		DateTimeOffset? ModifiedAt { get; set; }
		Guid? ModifiedById { get; set; }
	}
}