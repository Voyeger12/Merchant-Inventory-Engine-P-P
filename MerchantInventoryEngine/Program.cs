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

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Services.AppLogger.Info("Application startup");
        Application.Run(new Form1());
    }    
}