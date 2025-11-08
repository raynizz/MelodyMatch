using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class EmptyFileExceptions : BusinessException
{
    public EmptyFileExceptions(string errorCode) : base(errorCode)
    {
    }
    
}