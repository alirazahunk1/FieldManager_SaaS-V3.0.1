namespace ESSWebPortal.ACL
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}
