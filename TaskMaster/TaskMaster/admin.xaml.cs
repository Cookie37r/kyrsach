using System;
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

namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        private Tasks tasks;
        Users user = new Users();
        private int userId;
        private string selectedUser = "All";
        private string selectedType = "All";
        private string selectedStatus = "All";

        public admin(DataBase db)
        {
            InitializeComponent();
            this.tasks = new Tasks(db);
            tasks.LoadComboBox(typeCb, statusCb);
            tasks.LoadUserComboBox(userCb);
            FillTaskList();
        }

        private void FillTaskList()
        {
                List<Tasks> taskList = tasks.GetTasks(-1, selectedType, selectedStatus);
                TaskList.ItemsSource = taskList;
        }

        private void exitakk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            this.Hide();
            mn.Show();
        }

        private void resbt_Click(object sender, RoutedEventArgs e)
        {
            Grid resgrid = (Grid)((Button)sender).FindName("resgrid");
            if (resgrid != null)
            {
                if (resgrid.Visibility == Visibility.Collapsed)
                {
                    resgrid.Visibility = Visibility.Visible;
                }
                else
                {
                    resgrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void serchbt_Click(object sender, RoutedEventArgs e)
        {
            this.selectedUser = string.IsNullOrEmpty(userCb.Text) ? "All" : userCb.Text;
            this.selectedType = string.IsNullOrEmpty(typeCb.Text) ? "All" : typeCb.Text;
            this.selectedStatus = string.IsNullOrEmpty(statusCb.Text) ? "All" : statusCb.Text;

            string selectedUser = userCb.Text; // Получить выбранного пользователя из ComboBox

            // Используйте метод GetUserFromComboBox для получения ID пользователя на основе его имени
            int selectedUserId = tasks.GetUserFromComboBox(selectedUser);

            // Вызываем обновление списка задач с учетом фильтра
            List<Tasks> filteredTasks = tasks.GetTasks(selectedUserId, this.selectedType, this.selectedStatus);
            TaskList.ItemsSource = filteredTasks;
        }


        private void resetFilterbt_Click(object sender, RoutedEventArgs e)
        {
            typeCb.Text = string.Empty;
            statusCb.Text = string.Empty;
            userCb.Text = string.Empty;
            selectedUser = "All";
            selectedType = "All";
            selectedStatus = "All";
            FillTaskList();
        }

        private void taskbt_Click(object sender, RoutedEventArgs e)
        {
            TaskAdd tasadd = new TaskAdd();
            tasadd.Show();
        }

        private void userbt_Click(object sender, RoutedEventArgs e)
        {
            UsersAdd usadd = new UsersAdd();
            usadd.Show();
        }

        private void adminbt_Click(object sender, RoutedEventArgs e)
        {
            AdminAdd adadd = new AdminAdd();
            adadd.Show();
        }
    }
}
