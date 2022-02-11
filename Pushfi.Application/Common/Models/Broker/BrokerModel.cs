using Microsoft.AspNetCore.Http;
using Pushfi.Common.Constants.User;
using Pushfi.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pushfi.Application.Common.Models.Broker
{
    public class BrokerModel
    {
        #region ApplicationUser
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        #endregion

        #region Broker
        [Required]
        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string UrlPath { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
        public string MobilePhoneNumber { get; set; }


        [MaxLength(UserEntityConstants.NameMaxLength)]
        public string CompanyName { get; set; }


        [MaxLength(UserEntityConstants.PhoneNumberMaxLenght)]
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

        public EntityImageModel LogoImage { get; set; }

        [MaxFileSize(2 * 1024 * 1024)] // 2mb
        [AllowedExtensions(new string[] { ".png", ".jpeg", ".jpg" })]
        public IFormFile LogoImageFile { get; set; }

        [Required]
        [MaxLength(100)]
        public string ESignature { get; set; }

        [Required]
        [MaxLength(UserEntityConstants.WebsiteURLMaxLenght)]
        public string BusinessWebsiteURL { get; set; }

        public EntityFileModel AdditionalDocument { get; set; }

        [MaxFileSize(5 * 1024 * 1024)] // 5mb
        [AllowedExtensions(new string[] { ".doc", ".docx", ".ppt", ".ppsx", ".pptx", ".xlsx", ".pdf" })]
        public IFormFile AdditionalDocumentFile { get; set; }

        [Required]
        [Range(7.9, 14.9)]
        public double BackEndFee { get; set; }
        #endregion
    }
}
