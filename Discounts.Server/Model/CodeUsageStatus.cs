namespace Discounts.Server.Model
{
    public enum CodeUsageStatus
    {
        Success = 0,
        CodeInvalid = 1,
        CodeAlreadyUsed = 2,
        CodeUsageError = 3,
        UnexpectedError = 4
    }
}
