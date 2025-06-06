﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for DeleteGroupWindow.xaml
    /// </summary>
    public partial class DeleteGroupWindow : Window
    {
        public DeleteGroupWindow()
        {
            InitializeComponent();
            lblDeleteCode.Content = GroupState.LoadedGroup.InviteCode;
        }

        // Check: Gebruiker moet exacte code ingeven, anders is de knop niet enabled voor te deleten 
        private void txtConfirmDelete_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnDelete.IsEnabled = txtConfirmDelete.Text.Trim().Equals(GroupState.LoadedGroup.InviteCode, StringComparison.OrdinalIgnoreCase);
        }

        //Als knop klikbaar --> Dubbel checken of echt wel hetzelfde, daarna verwijderen
        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (txtConfirmDelete.Text.Trim().Equals(GroupState.LoadedGroup.InviteCode, StringComparison.OrdinalIgnoreCase)){
                GroupService.DeleteGroup(GroupState.LoadedGroup.Id);
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
