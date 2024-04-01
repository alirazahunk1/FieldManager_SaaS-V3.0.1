namespace ESSCommon.Core.Subscription
{
    public interface ISubscription
    {
        Task<bool> IsSubscriptionActive(int tenantId);
    }
}
