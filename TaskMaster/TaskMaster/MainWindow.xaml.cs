using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Users user = new Users();
        Admin admin = new Admin();
        DataBase database = new DataBase();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginbt_Click(object sender, RoutedEventArgs e)
        {
            string login = logintb.Text;
            string password = passwordtb.Password;

            bool isAdmin = admin.AuthorizationAdmin(login, password);
            if (isAdmin)
            {
                admin ad = new admin(database);
                this.Hide();
                ad.Show();
            }
            else
            {
                int userId = user.AuthenticateUser(login, password);

                if (userId != -1)
                {
                    task ts = new task(userId, database);
                    this.Hide();
                    ts.Show();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль. Попробуйте снова.", "Ошибка при попытке авторизоваться", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void svernutbt_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closebt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            o_prog oprg = new o_prog();
            oprg.Show();
        }
    }
}
