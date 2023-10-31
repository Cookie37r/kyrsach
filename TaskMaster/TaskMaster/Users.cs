//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Windows;

//namespace TaskMaster
//{

//    public class Users
//    {
//        private int id_user;
//        private string login;
//        private string password;
//        private string fullName;
//        private string image;
//        private string dolzhnost;

//        DataBase database = new DataBase();

//        public bool AuthorizationUser(string login, string password)
//        {
//            try
//            {
//                using (SqlCommand chekuser = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Login = @Login AND Password = @Password", database.getConnection()))
//                {
//                    chekuser.Parameters.AddWithValue("@Login", login);
//                    chekuser.Parameters.AddWithValue("@Password", password);

//                    int countus = (int)chekuser.ExecuteScalar();

//                    // Если есть совпадение, вернуть true
//                    return countus > 0;
//                }
//            }
//            catch (Exception ex)
//            {
//                // Обработать исключение, например, записать его в лог
//                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
//                return false;
//            }
//        }



//        public void AddUser()
//        {
//            throw new System.NotImplementedException();
//        }

//        public void DeleteUser()
//        {
//            throw new System.NotImplementedException();
//        }

//        public void SaveUser()
//        {
//            throw new System.NotImplementedException();
//        }
//    }

//    public class Admin
//    {
//        private int id_admin;
//        private string login;
//        private string password;

//        public void AuthorizationAdmin()
//        {
//            throw new System.NotImplementedException();
//        }
//    }

//    public class Tasks
//    {
//        private int id_task;
//        private string status;
//        private string sroc;
//        private string full_name;
//        private string location;
//        private string text;
//        private int id_user;
//        private int id_result;
//        private string time;
//        private int id_type;

//        public void AddTask()
//        {
//            throw new System.NotImplementedException();
//        }

//        public void DeleteTask()
//        {
//            throw new System.NotImplementedException();
//        }

//        public void SaveTask()
//        {
//            throw new System.NotImplementedException();
//        }

//        public void LoadTask()
//        {
//            throw new System.NotImplementedException();
//        }
//    }

//    public class Types
//    {
//        private int id_type;
//        private string type;
//    }

//    public class Result : Tasks
//    {
//        private int id_result;
//        private string comment;
//        private string file;
//        private string picture;

//        public void AddResult()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}