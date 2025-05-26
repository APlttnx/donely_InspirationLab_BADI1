using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using System.IO;
using donely_Inspilab.Pages;
using donely_Inspilab.Classes;

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

        //Configuratie via database, appsettings.json file zelf aan te maken voor custom database connectie (stel je hebt een andere poort, wachtwoord, naam, ...)
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    //public static HomePage HomePage = null;

    //when user not logged in
    public static void LogoutAndReset()
    {
        //App.HomePage = null;

        //leegmaken session en states
        SessionManager.Logout();
        GroupState.ClearGroup();

        //naar welcomepage
        App.MainFrame.Navigate(new WelcomePage());

    }

}


