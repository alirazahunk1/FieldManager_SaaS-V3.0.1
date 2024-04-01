namespace ESSWebPortal.Logger
{
    public interface ILoggerManager
    {
        public void LogDebug(string message);


        public void LogError(string message);


        public void LogInfo(string messagee);


        public void LogWar(string message);
    }
}
