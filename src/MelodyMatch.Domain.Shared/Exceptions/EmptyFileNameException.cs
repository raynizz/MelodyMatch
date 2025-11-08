using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class EmptyFileNameException : BusinessException
{
    public EmptyFileNameException(string errorCode) : base(errorCode)
    {
    }
}