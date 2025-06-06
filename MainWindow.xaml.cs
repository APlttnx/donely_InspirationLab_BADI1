﻿using donely_Inspilab.Classes;
using donely_Inspilab.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace donely_Inspilab;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        App.MainFrame = MainFrame;
        if (SessionManager.IsLoggedIn)
            MainFrame.Navigate(new HomePage());
        else
            MainFrame.Navigate(new WelcomePage()); //startpagina als de app opent
    }
}