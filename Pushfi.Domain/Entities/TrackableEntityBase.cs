using Pushfi.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities
{
    public class TrackableEntityBase : EntityBase, ITrackable
    {
        [Column(nameof(CreatedAt), Order = 1)]
        public virtual DateTimeOffset CreatedAt { get; set; }

        [Column(nameof(CreatedById), Order = 2)]
        public virtual Guid CreatedById { get; set; }

        [Column(nameof(ModifiedAt), Order = 3)]
        public virtual DateTimeOffset? ModifiedAt { get; set; }

        [Column(nameof(ModifiedById), Order = 4)]
        public virtual Guid? ModifiedById { get; set; }
    }
}
