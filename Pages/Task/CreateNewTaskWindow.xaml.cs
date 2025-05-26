using donely_Inspilab.Classes;
using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace donely_Inspilab.Pages.Task
{
    /// <summary>
    /// Interaction logic for CreateNewTaskWindow.xaml
    /// </summary>
    public partial class CreateNewTaskWindow : Window
    {
        public CreateNewTaskWindow()
        {
            InitializeComponent();
            //Zetten van frequency, standaard None
            cmbFrequency.ItemsSource = System.Enum.GetValues(typeof(TaskFrequency));
            cmbFrequency.SelectedIndex = 0; // default
        }
        public Classes.Task NewTask { get; private set; }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //input validation -> Empty fields
                if (string.IsNullOrEmpty(txtTaskName.Text))
                    throw new ArgumentException("Not all required fields are filled in.");
                NewTask = TaskService.CreateTask(txtTaskName.Text, txtDescription.Text, Convert.ToInt32(txtReward.Text), (TaskFrequency)cmbFrequency.SelectedItem, (bool)cbValidation.IsChecked, GroupState.LoadedGroup.Id);
                if (NewTask != null)
                {
                    MessageBox.Show("Task successfully created", "Success", MessageBoxButton.OK);
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (ArgumentException ex) //Voor als iets niet in orde met velden
            {
                MessageBox.Show(ex.Message, "Task not created", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (OverflowException ex) //Voor als iets niet in orde met velden
            {
                MessageBox.Show("Entered value is too big", "Overflow error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please fill in a whole number for reward", "Overflow error", MessageBoxButton.OK, MessageBoxImage.Warning); // zou normaal gezien niet mogen voorvallen
            }
            catch (Exception ex) //Voor als iets niet in orde met velden
            {
                MessageBox.Show("Something unexpected went wrong: " + ex.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        

        private static readonly Regex _regex = new Regex("^[0-9]+$");


        //Voor enkel nummers in te voegen bij Reward
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void NumberOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.DataObject.GetData(DataFormats.Text);
                if (!_regex.IsMatch(text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }

    }
}
