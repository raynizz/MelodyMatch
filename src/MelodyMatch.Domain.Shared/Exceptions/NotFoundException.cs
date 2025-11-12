using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class NotFoundException : BusinessException
{
    public NotFoundException(string errorCode) : base(errorCode)
    {
    }
}