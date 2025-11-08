using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class InvalidFileExtensionException : BusinessException
{
    public InvalidFileExtensionException(string errorCode) : base(errorCode)
    {
    }
}