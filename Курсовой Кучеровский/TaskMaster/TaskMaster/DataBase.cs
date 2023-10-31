using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace TaskMaster
{
    public class DataBase
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-S21HR1A;Initial Catalog=organization;Integrated Security=True");

        public void openConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }

        }

        public void closeConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

        }

        public SqlConnection getConnection()
        {
            return con;
        }
    }

    public class Users
    {
        public int id_user { get; private set; }
        public string login { get; private set; }
        public string password { get; private set; }
        public string fullName { get; set; }
        public string image { get; private set; }
        public string dolzhnost { get; private set; }
        public int svUserId { get; private set; }
        DataBase database = new DataBase();

        public int AuthenticateUser(string login, string password)
        {
            int userId = -1;

            database.openConnection();
            SqlCommand getUserInfo = new SqlCommand("SELECT ID_user FROM Users WHERE Login = @Login AND Password = @Password", database.getConnection());
            getUserInfo.Parameters.AddWithValue("@Login", login);
            getUserInfo.Parameters.AddWithValue("@Password", password);

            using (SqlDataReader reader = getUserInfo.ExecuteReader())
            {
                if (reader.Read())
                {
                    userId = reader.GetInt32(0);
                }
            }

            database.closeConnection();
            return userId;
        }

        public Users GetUserInfo(int userId)
        {
            Users userInfo = null;
            database.openConnection();
            SqlCommand getUserInfo = new SqlCommand("SELECT * FROM Users WHERE ID_user = @UserId", database.getConnection());
            getUserInfo.Parameters.AddWithValue("@UserId", userId);

            using (SqlDataReader reader = getUserInfo.ExecuteReader())
            {
                if (reader.Read())
                {
                    userInfo = new Users
                    {
                        id_user = reader.GetInt32(reader.GetOrdinal("ID_user")),
                        fullName = reader.GetString(reader.GetOrdinal("Full_name")),
                        image = reader.GetString(reader.GetOrdinal("image")),
                        dolzhnost = reader.GetString(reader.GetOrdinal("Dolzhnost")),
                    };
                }
            }

            database.closeConnection();
            return userInfo;
        }

        public void AddUser()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteUser()
        {
            throw new System.NotImplementedException();
        }

        public void SaveUser()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Admin
    {
        private int id_admin;
        private string login;
        private string password;
        DataBase database = new DataBase();

        public bool AuthorizationAdmin(string login, string password)
        {
            database.openConnection();
            SqlCommand chekadmin = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE Login = @Login " +
                "AND Password = @Password", database.getConnection());
            chekadmin.Parameters.AddWithValue("@Login", login);
            chekadmin.Parameters.AddWithValue("@Password", password);
            int countad = (int)chekadmin.ExecuteScalar();
            database.closeConnection();
            return countad > 0;
        }
    }

    public class Result
    {
        public int Id { get; set; }
        public string FileResult { get; set; }
        public string PhotoResult { get; set; }
        public string Comment { get; set; }
    }

    public class Tasks
    {
        private SqlConnection connection; // Поле для хранения соединения
        private DataBase database;

        public int id_task { get; set; }
        public string status { get; set; }
        public DateTime sroc { get; set; }
        public string location { get; set; }
        public string fullName { get; set; }
        public string text { get; set; }
        public int id_user { get; set; }
        public DateTime time { get; set; }
        public string id_type { get; set; }
        public int id_result { get; set; }
        public string comment { get; set; }
        public string fileResult { get; set; }
        public string photoResult { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsSelected { get; set; }


        public Tasks(DataBase db)
        {
            this.database = db;
            this.connection = db.getConnection();
        }

        public List<Tasks> GetTasks(int userId, string selectedType, string selectedStatus)
        {
            List<Tasks> userTasks = new List<Tasks>();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "SELECT ID_task, Text, Types.Type, Time, Sroc, Status, Users.Full_name, Location, " +
                "ISNULL(Results.Comment, '') AS Comment, ISNULL(Results.[File], '') AS [File], ISNULL(Results.Picture, '') AS Picture  FROM Tasks " +
                "LEFT JOIN Results ON Tasks.ID_result = Results.ID_result " +
                "JOIN Types ON Tasks.ID_type = Types.ID_type " +
                "JOIN Users ON Tasks.ID_user = Users.ID_user " +
                "WHERE (Tasks.ID_user = @UserId OR @UserId = -1) " +
                "AND (Types.Type = @SelectedType OR @SelectedType = 'All') " +
                "AND (Status = @SelectedStatus OR @SelectedStatus = 'All')";


            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@SelectedType", selectedType);
                command.Parameters.AddWithValue("@SelectedStatus", selectedStatus);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Загрузка задач
                        Users users = new Users();
                        Tasks task = new Tasks(database);
                        task.id_task = reader.GetInt32(0);
                        task.text = reader.GetString(1);
                        task.id_type = reader.GetString(2);
                        task.time = reader.GetDateTime(3);
                        task.sroc = reader.GetDateTime(4);
                        task.status = reader.GetString(5);
                        task.fullName = reader.GetString(6);
                        task.location = reader["Location"] as string;
                        task.comment = reader.GetString(8);
                        task.fileResult = reader.GetString(9);
                        task.photoResult = reader.GetString(10);


                        // Определение, является ли задача просроченной
                        DateTime currentDateTime = DateTime.Now;
                        if (task.status != "Выполнено" && task.sroc < currentDateTime)
                        {
                            task.IsOverdue = true;
                        }
                        else
                        {
                            task.IsOverdue = false;
                        }
                        task.IsSelected = false;
                        userTasks.Add(task);
                    }
                }
            }

            return userTasks;
        }

        public void LoadComboBox(ComboBox typeComboBox, ComboBox statusComboBox)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // Загрузка типов задач
            string queryType = "SELECT * FROM Types";
            using (SqlCommand commandType = new SqlCommand(queryType, connection))
            {
                using (SqlDataReader readerType = commandType.ExecuteReader())
                {
                    if (readerType.HasRows)
                    {
                        while (readerType.Read())
                        {
                            typeComboBox.Items.Add(readerType["Type"]);
                        }
                    }
                }
            }

            // Загрузка статусов задач
            string queryStatus = "SELECT DISTINCT Status FROM Tasks";
            using (SqlCommand commandStatus = new SqlCommand(queryStatus, connection))
            {
                using (SqlDataReader readerStatus = commandStatus.ExecuteReader())
                {
                    if (readerStatus.HasRows)
                    {
                        while (readerStatus.Read())
                        {
                            string statusName = readerStatus["Status"] as string;
                            statusComboBox.Items.Add(statusName);
                        }
                    }
                }
            }
        }

        public void LoadUserComboBox(ComboBox userComboBox)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // Загрузка списка сотрудников
            string queryUsers = "SELECT Full_name FROM Users";
            using (SqlCommand commandUsers = new SqlCommand(queryUsers, connection))
            {
                using (SqlDataReader readerUsers = commandUsers.ExecuteReader())
                {
                    if (readerUsers.HasRows)
                    {
                        while (readerUsers.Read())
                        {
                            userComboBox.Items.Add(readerUsers["Full_name"]);
                        }
                    }
                }
            }
        }

        public int GetUserFromComboBox(string selectedUser)
        {
            if (string.IsNullOrEmpty(selectedUser) || selectedUser == "All")
            {
                return -1; // Возвращаем -1, чтобы указать, что нужно показать все задачи (без фильтрации по пользователю).
            }
            else
            {
                // Выполните SQL-запрос, чтобы получить ID_user на основе имени пользователя (selectedUser).
                string query = "SELECT ID_user FROM Users WHERE Full_name = @UserName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", selectedUser);
                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void UpdateTaskStatus(int taskId, string newStatus)
        {
            database.openConnection();
            // Выполните SQL-запрос для обновления статуса задания в базе данных.
            string query = "UPDATE Tasks SET Status = @NewStatus WHERE ID_task = @TaskId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NewStatus", newStatus);
                command.Parameters.AddWithValue("@TaskId", taskId);

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }


        public void UpdateTaskStatusAndResult(int taskId, string newStatus, int resultId)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            // Выполняем SQL-запрос для обновления статуса и ID_result в таблице Tasks
            string updateQuery = "UPDATE Tasks SET Status = @NewStatus, ID_result = @ResultId WHERE ID_task = @TaskId";

            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@NewStatus", newStatus);
                updateCommand.Parameters.AddWithValue("@ResultId", resultId);
                updateCommand.Parameters.AddWithValue("@TaskId", taskId);

                updateCommand.ExecuteNonQuery();
            }
        }

        public int AddResultToDatabase(string comment, string fileResult, string photoResult)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            int resultId = -1; // Идентификатор результата

            // Выполняем SQL-запрос для добавления результатов в таблицу Results
            string insertQuery = "INSERT INTO Results (Comment, [File], Picture) VALUES (@Comment, @FileResult, @PhotoResult); SELECT SCOPE_IDENTITY();";

            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@Comment", comment);
                insertCommand.Parameters.AddWithValue("@FileResult", fileResult);
                insertCommand.Parameters.AddWithValue("@PhotoResult", photoResult);

                // Получаем идентификатор только что добавленной записи
                resultId = Convert.ToInt32(insertCommand.ExecuteScalar());
            }

            return resultId;
        }



        public void AddTask()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteTask()
        {
            throw new System.NotImplementedException();
        }

        public void SaveTask()
        {
            throw new System.NotImplementedException();
        }

        public void LoadTask()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Types
    {
        private int id_type;
        private string type;
    }


}
