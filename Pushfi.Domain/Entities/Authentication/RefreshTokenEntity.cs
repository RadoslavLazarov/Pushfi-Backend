using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities.Authentication
{
    public class RefreshTokenEntity : EntityBase
    {
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string CreatedByIp { get; set; }

        public DateTimeOffset? Revoked { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public string ReasonRevoked { get; set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;

        public bool IsRevoked => Revoked != null;

        public bool IsActive => !IsRevoked && !IsExpired;
    }
}