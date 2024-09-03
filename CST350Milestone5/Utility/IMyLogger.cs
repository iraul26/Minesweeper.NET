using NLog;

namespace CST350Milestone5.Utility
{
    public interface IMyLogger
    {
        private static Logger? logger;

        public void Debug(string message);
        public void Info(string message);
        public void Warning(string message);
        public void Error(string message);
    }
}
