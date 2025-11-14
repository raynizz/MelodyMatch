using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class FileCountException : BusinessException
{
    public FileCountException(string errorCode) : base(errorCode)
    {
    }
}