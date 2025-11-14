namespace MelodyMatch;

public static class MelodyMatchDomainErrorCodes
{
    public static class File
    {
        public const string FileIsEmpty = "MelodyMatch:File:00001";
        public const string FileSizeExceedsLimit = "MelodyMatch:File:00002";
        public const string InvalidFileExtension = "MelodyMatch:File:00003";
        public const string EmptyFileName = "MelodyMatch:File:00004";
    }
    
    public static class UserProfile
    {
        public const string UserProfileNotFilled = "MelodyMatch:UserProfile:00001";
        public const string ProfilePhotoLimitExceeded = "MelodyMatch:UserProfile:00002";
    }
}
