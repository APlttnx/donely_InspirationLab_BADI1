using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace donely_Inspilab;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static Frame MainFrame { get; set; }

    // Tools -> NuGet packet manager -> deze commando's uitvoeren
    /* Install-Package Microsoft.Extensions.Configuration
        Install-Package Microsoft.Extensions.Configuration.Json
        Install-Package Microsoft.Extensions.Configuration.Binder
     */

    public static IConfiguration Configuration { get; private set; }
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

}


