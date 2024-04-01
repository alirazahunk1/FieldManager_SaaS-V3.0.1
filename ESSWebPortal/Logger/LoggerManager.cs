using NLog;

namespace ESSWebPortal.Logger
{
    public class LoggerManager : ILoggerManager
    {

        private static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message) => _logger.Debug(message);

        public void LogError(string message) => _logger.Error(message);

        public void LogInfo(string message) => _logger.Info(message);

        public void LogWar(string message) => _logger.Warn(message);
    }
}
