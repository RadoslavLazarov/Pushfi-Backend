using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities
{
	public abstract class EntityBase
	{
		[Column(nameof(Id), Order = 0)]
		[Key]
		public Guid Id { get; set; }
	}
}
