using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для TaskAdd.xaml
    /// </summary>
    public partial class TaskAdd : Window
    {
        DataBase database = new DataBase();
        DataTable TaskTable;
        private Tasks tasks;


        public TaskAdd()
        {
            InitializeComponent();
            LoadData();
            this.tasks = new Tasks(database);
            tasks.LoadComboBox(typeCb, statusCb);
            tasks.LoadUserComboBox(userCb);
        }

        private void LoadData()
        {
            database.openConnection();

            TaskTable = new DataTable();

            // Загрузка данных из таблицы Tasks, а также соответствующих таблиц Users и Types
            string query = "SELECT Tasks.ID_task, Tasks.Text, Types.Type, Users.Full_name, Tasks.Time, Tasks.Sroc, Tasks.Status, Tasks.Location " +
                           "FROM Tasks " +
                           "JOIN Types ON Tasks.ID_type = Types.ID_type " +
                           "JOIN Users ON Tasks.ID_user = Users.ID_user";

            SqlCommand TasksCommand = new SqlCommand(query, database.getConnection());
            SqlDataAdapter TasksAdapter = new SqlDataAdapter(TasksCommand);
            TasksAdapter.Fill(TaskTable);

            dataGridTask.ItemsSource = TaskTable.DefaultView;

            database.closeConnection();
        }


        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database.openConnection();
                string text = texttb.Text;
                string queryType = "SELECT ID_type FROM Types WHERE Type = @Type";
                SqlCommand commandType = new SqlCommand(queryType, database.getConnection());
                commandType.Parameters.AddWithValue("@Type", typeCb.Text);
                int typeId = (int)commandType.ExecuteScalar();
                string queryUser = "SELECT ID_user FROM Users WHERE Full_name = @Users";
                SqlCommand commandUser = new SqlCommand(queryUser, database.getConnection());
                commandUser.Parameters.AddWithValue("@Users", userCb.Text);
                int userId = (int)commandUser.ExecuteScalar();
                string location = locationtb.Text;
                DateTime time = DateTime.Now;
                string status = statusCb.Text;
                DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue; // Выбранная дата из DatePicker
                string timeInput = timeTextBox.Text; // Время введенное в TextBox
                DateTime combinedDateTime;

                if (DateTime.TryParse(timeInput, out DateTime parsedTime))
                {
                    combinedDateTime = selectedDate.Date.Add(parsedTime.TimeOfDay);
                }
                else
                {
                    // Обработка ошибки ввода времени
                    MessageBox.Show("Неверный формат времени.");
                    return; // Выход из метода, чтобы избежать сохранения данных
                }
                database.closeConnection();

                // Добавление новой строки во временную таблицу productsTable
                DataRow newRow = TaskTable.NewRow();
                newRow["Text"] = text;
                newRow["Type"] = typeId;
                newRow["Full_name"] = userId;
                newRow["Location"] = location;
                newRow["Time"] = time;
                newRow["Sroc"] = combinedDateTime;
                newRow["Status"] = status;

                TaskTable.Rows.Add(newRow);

                // Очистка полей ввода
                texttb.Text = string.Empty;
                typeCb.Text = string.Empty;
                userCb.Text = string.Empty;
                locationtb.Text = string.Empty;
                datePicker.Text = string.Empty;

            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = dataGridTask.SelectedItem as DataRowView;
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
                    TaskTable.Rows.Remove(selectedRow.Row);
                }
            }
            else
            {
                MessageBox.Show("Выберите хотя бы одно значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database.openConnection();

                // Удаление строк из базы данных, соответствующих помеченным как удаленным строкам в productsTable
                foreach (DataRow row in TaskTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        int id = Convert.ToInt32(row["ID_task", DataRowVersion.Original]);
                        string query = "DELETE FROM Tasks WHERE ID_task = @ID";
                        SqlCommand command = new SqlCommand(query, database.getConnection());
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }

                // Добавление новых строк в базу данных
                string insertQuery = "INSERT INTO Tasks (Text, ID_type, ID_user, Time, Sroc, Status, Location) " +
                                     "VALUES (@Text, @ID_type, @ID_user, @Time, @Sroc, @Status, @Location)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, database.getConnection());
                insertCommand.Parameters.Add("@Text", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@ID_type", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@ID_user", SqlDbType.VarChar);
                insertCommand.Parameters.AddWithValue("@Time", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Sroc", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Status", SqlDbType.VarChar);
                insertCommand.Parameters.Add("@Location", SqlDbType.VarChar);

                foreach (DataRow row in TaskTable.Rows)
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        insertCommand.Parameters["@Text"].Value = row["Text"];
                        insertCommand.Parameters["@ID_type"].Value = row["Type"];
                        insertCommand.Parameters["@ID_user"].Value = row["Full_name"];
                        insertCommand.Parameters["@Time"].Value = row["Time"];
                        insertCommand.Parameters["@Sroc"].Value = row["Sroc"];
                        insertCommand.Parameters["@Status"].Value = row["Status"];
                        insertCommand.Parameters["@Location"].Value = row["Location"];
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

        private void ExportToExcel()
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Worksheet wsh;
            exApp.Workbooks.Add();
            wsh = (Excel.Worksheet)exApp.ActiveSheet;
            wsh.Name = "Задачи";

            // Получаем сегодняшнюю дату и форматируем ее
            string todayDate = DateTime.Now.ToString("dd.MM.yyyy");

            // Задаем ячейку для заголовка и объединяем ее с ячейками столбцов
            Excel.Range titleRange = (Excel.Range)wsh.Cells[1, 1];
            titleRange.Value = $"Список задач на {todayDate}";
            titleRange.Font.Size = 16; // Размер шрифта заголовка
            titleRange.Font.Bold = true; // Жирный шрифт
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            int columnCount = dataGridTask.Columns.Count;
            Excel.Range headerRange = (Excel.Range)wsh.Cells[1, 1];
            Excel.Range lastHeaderCell = (Excel.Range)wsh.Cells[1, columnCount];
            Excel.Range columnsRange = wsh.get_Range(headerRange, lastHeaderCell);
            columnsRange.Merge(); // Объединяем ячейки заголовка

            // Записываем заголовки столбцов с форматированием
            for (int j = 0; j < dataGridTask.Columns.Count; j++)
            {
                Excel.Range cell = (Excel.Range)wsh.Cells[2, j + 1];
                cell.Value = dataGridTask.Columns[j].Header;
                cell.Font.Bold = true; // Жирный шрифт
                cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }

            // Получаем DataView из DataGrid
            DataView dataView = (DataView)dataGridTask.ItemsSource;

            // Записываем данные с форматированием
            for (int i = 0; i < dataView.Count; i++)
            {
                for (int j = 0; j < dataGridTask.Columns.Count; j++)
                {
                    Excel.Range dataRange = (Excel.Range)wsh.Cells[i + 3, j + 1];
                    var cellValue = dataView[i][dataGridTask.Columns[j].SortMemberPath].ToString();
                    dataRange.Value = cellValue;
                }
            }

            wsh.Columns.AutoFit();
            wsh.Rows.AutoFit();
            exApp.Visible = true;
        }



        private void export_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }
    }
}
