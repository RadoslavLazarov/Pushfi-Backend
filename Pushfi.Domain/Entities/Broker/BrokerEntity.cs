using Microsoft.EntityFrameworkCore;
using Pushfi.Common.Constants.User;
using Pushfi.Domain.Entities.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities.Broker
{
    public class BrokerEntity : EntityBase
    {
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public Guid UserId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public string UrlPath { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
        [Column(TypeName = UserEntityConstants.PhoneNumberTypeName)]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string LastName { get; set; }

        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string CompanyName { get; set; }

        [MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
        [Column(TypeName = UserEntityConstants.PhoneNumberTypeName)]
        public string CompanyPhoneNumber { get; set; }

        [MaxLength(UserEntityConstants.TaxMaxLength)]
        public string TAXID { get; set; }

        [Required]
        [MaxLength(1000)]
        public string DisbursementAccountInfo { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.WebsiteURLMaxLenght)]
        public string WebsiteURL { get; set; }

        [MaxLength(50)]
        public string BrandingType { get; set; }

        public EntityImage LogoImage { get; set; }

        [Required]
        [MaxLength(100)]
        public string ESignature { get; set; }

        [MaxLength(UserEntityConstants.WebsiteURLMaxLenght)]
        public string BusinessWebsiteURL { get; set; }

        public EntityFile AdditionalDocument { get; set; }

        [Required]
        public double BackEndFee { get; set; }
    }
}
