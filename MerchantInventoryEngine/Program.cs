using System.Diagnostics;
using System.Threading;

namespace MerchantInventoryEngine;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            Services.AppLogger.Error("Unhandled domain exception", args.ExceptionObject as Exception);
        };

        Application.ThreadException += (_, args) =>
        {
            Services.AppLogger.Error("Unhandled UI thread exception", args.Exception);
        };

        ApplicationConfiguration.Initialize();

        using var splash = new SplashForm();
        try
        {
            Services.AppLogger.Info("Application startup");

            splash.Show();
            var splashTimer = Stopwatch.StartNew();
            splash.SetStatus("Loading modules...");

            splash.SetStatus("Connecting to local database...");
            var db = new Data.DatabaseHelper();

            splash.SetStatus("Running startup health checks...");
            if (!db.IsDatabaseHealthy())
            {
                throw new InvalidOperationException("Database health check failed.");
            }

            splash.SetStatus("Preparing interface...");
            const int minSplashMs = 1200;
            var remainingMs = minSplashMs - (int)splashTimer.ElapsedMilliseconds;
            if (remainingMs > 0)
            {
                Thread.Sleep(remainingMs);
            }
            splash.Hide();

            Application.Run(new Form1(db, performHealthCheck: false));
        }
        catch (Exception ex)
        {
            Services.AppLogger.Error("Startup failed.", ex);
            try
            {
                splash.Hide();
            }
            catch
            {
                // ignored
            }

            MessageBox.Show(
                "The application could not start. Please check logs/app.log for details.",
                "Startup error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }    
}