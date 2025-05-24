using donely_Inspilab.Enum;
using donely_Inspilab.Classes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace donely_Inspilab.Pages.Task {
    public partial class EditTaskWindow : Window
    {
        public Classes.Task TaskToEdit { get; private set; }
        public Classes.Task EditedTask { get; private set; }

        public EditTaskWindow(Classes.Task task)
        {
            InitializeComponent();
            // Set frequency combo items
            cmbFrequency.ItemsSource = System.Enum.GetValues(typeof(TaskFrequency));
            TaskToEdit = task;
            LoadItems();

            
        }

        private void LoadItems()
        {
            if (TaskToEdit == null) return;

            // Populate fields
            txtTaskName.Text = TaskToEdit.Name;
            txtDescription.Text = TaskToEdit.Description;
            txtReward.Text = TaskToEdit.Reward.ToString();
            cmbFrequency.SelectedItem = TaskToEdit.Frequency;
            cbValidation.IsChecked = TaskToEdit.RequiresValidation;
        }

        // Keep your number only input validation here
        private static readonly Regex _regex = new Regex("^[0-9]+$");

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

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTaskName.Text == TaskToEdit.Name //Check of iets is veranderd
                    && txtDescription.Text == TaskToEdit.Description
                    && Convert.ToInt32(txtReward.Text) == TaskToEdit.Reward
                    && (TaskFrequency)cmbFrequency.SelectedItem == TaskToEdit.Frequency
                    && (bool)cbValidation.IsChecked == TaskToEdit.RequiresValidation)
                {
                    throw new ArgumentException("No changes detected, update not required.");
                }
                if (string.IsNullOrEmpty(txtTaskName.Text))
                    throw new ArgumentException("Not all required fields are filled in.");

                int reward = Convert.ToInt32(txtReward.Text);
                if (reward < 0)
                    throw new ArgumentException("Reward cannot be a negative number.");

                // Assuming EditedTask.Id holds the current task's ID
                EditedTask=  TaskService.UpdateTask(
                    TaskToEdit.Id,
                    txtTaskName.Text,
                    txtDescription.Text,
                    reward,
                    (TaskFrequency)cmbFrequency.SelectedItem,
                    (bool)cbValidation.IsChecked);

                if (EditedTask != null)
                {
                    MessageBox.Show("Task successfully updated", "Success", MessageBoxButton.OK);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Task update failed", "Task not updated", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Task not updated", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Entered value is too big", "Overflow error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please fill in a whole number for reward", "Format error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something unexpected went wrong: " + ex.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
        }
    }
}
