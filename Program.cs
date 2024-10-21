using Microsoft.Extensions.DependencyInjection;
using TechBOM.Interfaces;

namespace TechBOM
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Create a new ServiceCollection
            var serviceCollection = new ServiceCollection();

            // Register dependencies
            ConfigureServices(serviceCollection);

            // Build the ServiceProvider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Run the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(serviceProvider.GetRequiredService<MainForm>());

            //Application.Run(new MainForm());
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Register RootNode and ExcelProcessor as transient
            services.AddTransient<RootNode>();
            services.AddTransient<ExcelProcessor>();

            // Register factories for RootNode and ExcelProcessor
            services.AddTransient<Func<RootNode>>(provider => () => provider.GetRequiredService<RootNode>());
            services.AddTransient<Func<ExcelProcessor>>(provider => () => provider.GetRequiredService<ExcelProcessor>());

            // Register the implementations of interfaces
            services.AddSingleton<ICatiaConnect, CatiaConnect>();
            services.AddTransient<ICounter, Counter>();

            // Register the MainForm
            services.AddTransient<MainForm>();
        }
    }
}