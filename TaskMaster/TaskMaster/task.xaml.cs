using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaskMaster
{
    /// <summary>
    /// Логика взаимодействия для task.xaml
    /// </summary>
    public partial class task : Window
    {
        private Tasks tasks;
        Users user = new Users();
        private int userId;
        private string selectedType = "All";
        private string selectedStatus = "All";

        public task(int userId, DataBase db)
        {
            InitializeComponent();
            this.userId = userId;
            this.tasks = new Tasks(db);
            FillUserInfo();
            tasks.LoadComboBox(typeCb, statusCb);
            FillTaskList();

        }

        private void FillUserInfo()
        {
            try
            {
                Users user = new Users();
                Users userInfo = user.GetUserInfo(userId);

                if (userInfo != null)
                {
                    Full_name.Text = userInfo.fullName;
                    Dolzhnost.Text = userInfo.dolzhnost;
                    ImageSourceConverter converter = new ImageSourceConverter();
                    staffImage.Source = (ImageSource)converter.ConvertFromString(userInfo.image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillTaskList()
        {
            try
            {
                List<Tasks> taskList = tasks.GetTasks(userId, selectedType, selectedStatus);
                TaskList.ItemsSource = taskList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void exitakk_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            this.Hide();
            mn.Show();
        }

        private void serchbt_Click(object sender, RoutedEventArgs e)
        {
            this.selectedType = string.IsNullOrEmpty(typeCb.Text) ? "All" : typeCb.Text;
            this.selectedStatus = string.IsNullOrEmpty(statusCb.Text) ? "All" : statusCb.Text;

            // Вызываем обновление списка задач с учетом фильтра
            List<Tasks> filteredTasks = tasks.GetTasks(userId, this.selectedType, this.selectedStatus);
            TaskList.ItemsSource = filteredTasks;
        }

        private void resetFilterbt_Click(object sender, RoutedEventArgs e)
        {
            typeCb.Text = string.Empty;
            statusCb.Text = string.Empty;
            selectedType = "All";
            selectedStatus = "All";
            FillTaskList();
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

        private void donebt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Tasks task in TaskList.Items)
                {
                    if (task.IsSelected)
                    {
                        // Здесь вы можете выполнить обновление статуса задачи
                        task.UpdateTaskStatus(task.id_task, "Выполнено");
                    }
                }

                // После обновления статусов, обновите список задач
                FillTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void sendbt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskList.SelectedItem != null)
                {
                    // Получить выбранную задачу
                    Tasks selectedTask = (Tasks)TaskList.SelectedItem;
                    string comment = selectedTask.comment;
                    string fileResult = selectedTask.fileResult;
                    string photoResult = selectedTask.photoResult;

                    selectedTask.photoResult = photoResult;

                    // Проверить, что текстовые поля не пустые
                    if (!string.IsNullOrEmpty(comment) || !string.IsNullOrEmpty(fileResult) || !string.IsNullOrEmpty(photoResult))
                    {
                        // Передать id_task выбранной задачи и вызвать метод для добавления результата в базу данных
                        int resultId = selectedTask.AddResultToDatabase(comment, fileResult, photoResult);

                        if (resultId > 0)
                        {
                            // Успешно добавлено, обновить статус и связать задачу с результатом
                            selectedTask.UpdateTaskStatusAndResult(selectedTask.id_task, "Выполнено", resultId);

                            MessageBox.Show("Результат успешно добавлен.");
                        }
                        else
                        {
                            // Произошла ошибка при добавлении
                            MessageBox.Show("Произошла ошибка при добавлении результата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // Вывести сообщение пользователю, что необходимо заполнить хотя бы одно поле
                        MessageBox.Show("Заполните хотя бы одно поле результата.");
                    }
                }
                else
                {
                    // Вывести сообщение пользователю, что задача не выбрана
                    MessageBox.Show("Выберите задачу для отправки результата.");
                }

                FillTaskList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void filebt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskList.SelectedItem != null)
                {
                    Tasks selectedTask = (Tasks)TaskList.SelectedItem;
                    OpenFileDialog opFD = new OpenFileDialog();
                    opFD.Filter = "ALL Files |*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.xlsm";
                    bool? res = opFD.ShowDialog();
                    if (res == true)
                    {
                        string originalFileName = opFD.SafeFileName; // Получить оригинальное имя файла
                        string targetFileName = Path.Combine(@"../../resultFile/", originalFileName); // Создать путь к целевому файлу

                        int counter = 1;
                        while (File.Exists(targetFileName))
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                            string fileExtension = Path.GetExtension(originalFileName);
                            string newFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                            targetFileName = Path.Combine(@"../../resultFile/", newFileName);
                            counter++;
                        }

                        File.Copy(opFD.FileName, targetFileName);

                        // Записываем относительный путь в filetb
                        selectedTask.fileResult = targetFileName;
                    }
                }
                else
                {
                    // Вывести сообщение пользователю, что задача не выбрана
                    MessageBox.Show("Выберите задачу для отправки результата.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void imgebt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskList.SelectedItem != null)
                {
                    Tasks selectedTask = (Tasks)TaskList.SelectedItem;
                    OpenFileDialog opFD = new OpenFileDialog();
                    opFD.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                    bool? res = opFD.ShowDialog();
                    if (res == true)
                    {
                        string originalFileName = opFD.SafeFileName; // Получить оригинальное имя файла
                        string targetFileName = Path.Combine(@"../../resultPhoto/", originalFileName); // Создать путь к целевому файлу

                        int counter = 1;
                        while (File.Exists(targetFileName))
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                            string fileExtension = Path.GetExtension(originalFileName);
                            string newFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                            targetFileName = Path.Combine(@"../../resultPhoto/", newFileName);
                            counter++;
                        }

                        File.Copy(opFD.FileName, targetFileName);
                        selectedTask.photoResult = targetFileName;
                    }
                }
                else
                {
                    // Вывести сообщение пользователю, что задача не выбрана
                    MessageBox.Show("Выберите задачу для отправки результата.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении фота: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
