using System;
using System.IO;

namespace MerchantInventoryEngine.Services
{
    public static class AppLogger
    {
        private static readonly object SyncRoot = new object();
        private static readonly string LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "app.log");

        public static void Info(string message)
        {
            Write("INFO", message, null);
        }

        public static void Error(string message, Exception? ex = null)
        {
            Write("ERROR", message, ex);
        }

        private static void Write(string level, string message, Exception? ex)
        {
            lock (SyncRoot)
            {
                Directory.CreateDirectory(LogDirectory);
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";
                File.AppendAllText(LogFilePath, line + Environment.NewLine);
                if (ex != null)
                {
                    File.AppendAllText(LogFilePath, ex + Environment.NewLine);
                }
            }
        }
    }
}