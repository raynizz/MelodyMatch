using Volo.Abp;

namespace MelodyMatch.Exceptions;

public class AccountBannedException : BusinessException
{
    public AccountBannedException(string errorCode) : base(errorCode)
    {
    }
}