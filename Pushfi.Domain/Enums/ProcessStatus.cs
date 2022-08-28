namespace Pushfi.Domain.Enums
{
    public enum ProcessStatus
    {
        None = 0,
        Registration = 1,
        Authentication = 2,
        GetOffer = 3,
        CreditApproved = 4,
        CreditFreeze = 5,
        CreditDecline = 6
    }
}
