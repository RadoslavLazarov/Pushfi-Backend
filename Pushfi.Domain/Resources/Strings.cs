namespace Pushfi.Domain.Resources
{
    public class Strings
    {
        #region Exceptions
        #region General
        public const string SomethingWentWrong = "Something went wrong!";
        #endregion

        #region Identity
        public const string UserDoesNotExsists = "User does not exsists!";
        public const string EmailAlreadyExsists = "Email already exsists!";
        public const string WrongPassword = "Wrong password!";
        public const string UserAccountDeleted = "User account is deleted!";
        public const string UserDeletionFailed = "User deletion failed!";
        #endregion

        #region Other
        public const string SpecifiedTypeIsNotEnum = "The specified type is not an enum!";
        public const string OnlyImagesAreAllowedToBeUploaded = "Only images are allowed to be uploaded!";
        public const string UrlPathExsists = "URL Path already exsists!";
        public const string FileUploadFailed = "File upload failed!";
        #endregion
        #endregion

        #region Enfortra errors
        public const string EnfortraUserExsists = "User already exists in the Enfortra platform, try an alternate email address.";
        #endregion
    }
}
