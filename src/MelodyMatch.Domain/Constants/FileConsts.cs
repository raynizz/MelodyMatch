namespace MelodyMatch.Constants;

public static class FileConsts
{
    public static class Avatar
    {
        public const int MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB

        public static readonly string[] AllowedExtensions =
        [
            ".jpg",
            ".jpeg",
            ".png"
        ];

        public const string AvatarFolderPath = "uploads/avatars";
        
        public const int AutoDeleteUnsavedAvatarsTimeHours = 6;
        
        public const string LogAvatarUploadsFileName = "temp-avatars.txt";
    }
}