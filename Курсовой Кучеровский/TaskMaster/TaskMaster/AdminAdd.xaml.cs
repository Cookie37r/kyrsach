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
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для AdminAdd.xaml
    /// </summary>
    public partial class AdminAdd : Window
    {
        DataBase database = new DataBase();
        DataTable AdminTable;
        private Admin admins;

        public AdminAdd()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            database.openConnection();

            // Загрузка данных из таблицы products
            AdminTable = new DataTable();
            SqlCommand productsCommand = new SqlCommand("SELECT * FROM Admin", database.getConnection());
            SqlDataAdapter productsAdapter = new SqlDataAdapter(productsCommand);
            productsAdapter.Fill(AdminTable);
            dataGridAdmin.ItemsSource = AdminTable.DefaultView;

            database.closeConnection();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            string login = logintb.Text;
            string password = passwordtb.Text;
            
            // Добавление новой строки во временную таблицу productsTable
            DataRow newRow = AdminTable.NewRow();
            newRow["Login"] = login;
            newRow["Password"] = password;
           

            AdminTable.Rows.Add(newRow);

            // Очистка полей ввода
            logintb.Text = string.Empty;
            passwordtb.Text = string.Empty;
            
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = dataGridAdmin.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                // Проверка, является ли строка новой (добавленной) или существующей
                if (selectedRow.Row.RowState != DataRowState.Added)
                {
                    // Если строка не новая, помечаем ее для удаления
                    selectedRow.Row.Delete();
                }
                else
                {
                    // Если строка новая, удаляем ее непосредственно из таблицы
                    AdminTable.Rows.Remove(selectedRow.Row);
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одно значение");
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database.openConnection();

                // Удаление строк из базы данных, соответствующих помеченным как удаленным строкам в productsTable
                foreach (DataRow row in AdminTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        int id = Convert.ToInt32(row["ID_admin", DataRowVersion.Original]);
                        string query = "DELETE FROM Admin WHERE ID_admin = @ID";
                        SqlCommand command = new SqlCommand(query, database.getConnection());
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }

                // Добавление новых строк в базу данных из productsTable
                string insertQuery = "INSERT INTO Admin (Login, Password) " +
                                     "VALUES (@Login, @Password)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, database.getConnection());
                insertCommand.Parameters.Add("@Login", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Password", SqlDbType.VarChar);

                foreach (DataRow row in AdminTable.Rows)
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        insertCommand.Parameters["@Login"].Value = row["Login"];
                        insertCommand.Parameters["@Password"].Value = row["Password"];
                        insertCommand.ExecuteNonQuery();
                    }
                }

                database.closeConnection();
                LoadData();
                MessageBox.Show("Изменения успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Обработка и отображение сообщения об ошибке
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
