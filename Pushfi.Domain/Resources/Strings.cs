namespace Pushfi.Domain.Resources
{
    public class Strings
    {
        #region Exceptions
        #region General
        public const string SomethingWentWrong = "Something went wrong!";
        #endregion

        #region Identity
        public const string InvalidToken = "Invalid Token!";
        public const string UserDoesNotExsists = "User does not exsists!";
        public const string WrongUser = "Wrong user!";
        public const string EmailAlreadyExsists = "Email already exsists!";
        public const string WrongPassword = "Wrong password!";
        public const string UserAccountDeleted = "User account is deleted!";
        public const string UserDeletionFailed = "User deletion failed!";
        public const string DefaultAdminCreationFailed = "Default User Admin creation failed!";
        #endregion

        #region Other
        public const string SpecifiedTypeIsNotEnum = "The specified type is not an enum!";
        public const string OnlyImagesAreAllowedToBeUploaded = "Only images are allowed to be uploaded!";
        public const string UrlPathExsists = "URL Path already exsists!";
        public const string FileUploadFailed = "File upload failed!";
        #endregion
        #endregion

        #region Enfortra errors
        public const string EnfortraError = "Enfortra error!";
        public const string EnfortraUserExsists = "User already exists in the Enfortra platform, try an alternate email address.";
        #endregion

        #region Email
        public const string CreditOfferScoreFactor = "<li style=\"margin-bottom: 20px;\"><div style=\"font-weight: 600; color: @@scoreTypeColor@@;\">@@scoreTypeName@@</div><div><b>Factor: </b>@@scoreFactor@@</div></li>";
        public const string BrokerRegistrationEmailSubject = "PushFi registration successful: {0}";
        public const string BrokerRegistrationEmailMessage = "Thank you for joining the PushFi team. This is your unique submission link - {0}/{1}/customer-apply";
        #endregion
    }
}
