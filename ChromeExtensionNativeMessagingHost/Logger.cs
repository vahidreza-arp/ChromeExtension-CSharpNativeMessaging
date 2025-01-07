using log4net.Config;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Logger
{
    public static class Log
    {
        public enum LogLevel
        {
            Info,
            Debug,
            Warn,
            Error
        }

        private static bool _initialized = false;
        private readonly static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(Log));

        public static void Init(string configPath)
        {
            XmlConfigurator.Configure(new FileInfo(configPath));
            _initialized = true;
        }

        private static void _Log(LogLevel level, string message)
        {
            if (!_initialized)
            {
                Init($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\LoggerConfig.config");
            }

            switch (level)
            {
                case LogLevel.Debug:
#if DEBUG
                    _logger.Debug(message);
#endif
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message);
                    break;
            }
        }

        public static void Info(string message)
        {
            var stackTrace = new StackTrace().GetFrame(1).GetMethod().Name;
            _Log(LogLevel.Info, $"[{stackTrace}] {message}");
        }

        public static void Error(string message)
        {
            var stackTrace = new StackTrace().GetFrame(1).GetMethod().Name;
            _Log(LogLevel.Error, $"[{stackTrace}] {message}");
        }

        public static void Debug(string message)
        {
            var stackTrace = new StackTrace().GetFrame(1).GetMethod().Name;
            _Log(LogLevel.Debug, $"[{stackTrace}] {message}");
        }

        public static void Warn(string message)
        {
            var stackTrace = new StackTrace().GetFrame(1).GetMethod().Name;
            _Log(LogLevel.Warn, $"[{stackTrace}] {message}");
        }
    }
}
