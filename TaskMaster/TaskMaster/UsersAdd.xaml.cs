using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;


namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для UsersAdd.xaml
    /// </summary>
    public partial class UsersAdd : Window
    {
        DataBase database = new DataBase();
        DataTable UsersTable;
        private Tasks tasks;

        public UsersAdd()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            database.openConnection();

            UsersTable = new DataTable();

            // Загрузка данных из таблицы Tasks, а также соответствующих таблиц Users и Types
            string query = "SELECT  * FROM Users";

            SqlCommand UserCommand = new SqlCommand(query, database.getConnection());
            SqlDataAdapter UserAdapter = new SqlDataAdapter(UserCommand);
            UserAdapter.Fill(UsersTable);
            dataGridUsers.ItemsSource = UsersTable.DefaultView;
            database.closeConnection();
        }


        private void delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = dataGridUsers.SelectedItem as DataRowView;
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
                    UsersTable.Rows.Remove(selectedRow.Row);
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одно значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = nametb.Text;
                string dolzhnost = dolzhnosttb.Text;
                string login = logintb.Text;
                string password = passwordtb.Text;
                string image = imgtb.Text;

                // Добавление новой строки во временную таблицу productsTable
                DataRow newRow = UsersTable.NewRow();
                newRow["Full_name"] = name;
                newRow["Dolzhnost"] = dolzhnost;
                newRow["Login"] = login;
                newRow["Password"] = password;
                newRow["image"] = image;

                UsersTable.Rows.Add(newRow);

                // Очистка полей ввода
                nametb.Text = string.Empty;
                dolzhnosttb.Text = string.Empty;
                logintb.Text = string.Empty;
                passwordtb.Text = string.Empty;
                imgtb.Text = string.Empty;
            }

            catch (Exception ex)
            {
                // Обработка и отображение сообщения об ошибке
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database.openConnection();

                // Удаление строк из базы данных, соответствующих помеченным как удаленным строкам в UsersTable
                foreach (DataRow row in UsersTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        int id = Convert.ToInt32(row["ID_user", DataRowVersion.Original]);
                        string query = "DELETE FROM Users WHERE ID_user = @ID";
                        SqlCommand command = new SqlCommand(query, database.getConnection());
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }

                // Добавление новых строк в базу данных из UsersTable
                string insertQuery = "INSERT INTO Users (Full_name, Dolzhnost, Login, Password, image) " +
                                     "VALUES (@Full_name, @Dolzhnost, @Login, @Password, @image)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, database.getConnection());
                insertCommand.Parameters.Add("@Full_name", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Dolzhnost", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Login", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Password", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@image", SqlDbType.VarChar);

                foreach (DataRow row in UsersTable.Rows)
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        insertCommand.Parameters["@Full_name"].Value = row["Full_name"];
                        insertCommand.Parameters["@Dolzhnost"].Value = row["Dolzhnost"];
                        insertCommand.Parameters["@Login"].Value = row["Login"];
                        insertCommand.Parameters["@Password"].Value = row["Password"];
                        insertCommand.Parameters["@image"].Value = row["image"];
                        insertCommand.ExecuteNonQuery();
                    }
                }

                database.closeConnection();
                LoadData();
                MessageBox.Show("Изменения успешно сохранены.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void imgbt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog opFD = new OpenFileDialog();
                opFD.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                bool? res = opFD.ShowDialog();
                if (res == true)
                {
                    string originalFileName = opFD.SafeFileName; // Получить оригинальное имя файла
                    string targetFileName = Path.Combine(@"../../pictures/", originalFileName); // Создать путь к целевому файлу

                    int counter = 1;
                    while (File.Exists(targetFileName))
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                        string fileExtension = Path.GetExtension(originalFileName);
                        string newFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                        targetFileName = Path.Combine(@"../../pictures/", newFileName);
                        counter++;
                    }

                    File.Copy(opFD.FileName, targetFileName);
                    imgtb.Text = targetFileName; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
