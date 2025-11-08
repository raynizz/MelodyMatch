namespace MelodyMatch;

public static class MelodyMatchDomainErrorCodes
{
    public static class Avatar
    {
        public const string FileIsEmpty = "MelodyMatch:File:00001";
        public const string FileSizeExceedsLimit = "MelodyMatch:File:00002";
        public const string InvalidFileExtension = "MelodyMatch:File:00003";
        public const string EmptyFileName = "MelodyMatch:File:00004";
    }
}
