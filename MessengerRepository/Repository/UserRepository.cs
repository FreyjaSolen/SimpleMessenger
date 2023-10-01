using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace MessengerRepository.Repository
{
    public class UserRepository
    {
        string ConnectionName { get; set; }

        public UserRepository(string connectionName)
        {
            ConnectionName = connectionName;
        }

        private DbConnection GetDbConnection()
        {
            var settings = ConfigurationManager.ConnectionStrings[ConnectionName];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = settings.ConnectionString;

            return connection;
        }

        public bool isLogExist(string login)
        {
            bool result = true;
            string query = $"SELECT * FROM passwords WHERE NickName = '{login}'";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                reader.Close();
            }

            return result;
        }

        public bool isPassExist(string userName, string password)
        {
            bool result = true;
            string query = $"SELECT * FROM passwords WHERE NickName = '{userName}' AND Password = '{password}'";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                reader.Close();
            }
            return result;
        }

        public int getUserID(string userName, string password)
        {
            int ID = -1;
            string query = $"SELECT id FROM passwords WHERE NickName = '{userName}' AND Password = '{password}'";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        ID = Int32.Parse(row["id"].ToString());
                    }
                }
                reader.Close();
            }

            return ID;
        }

        public int addUser(string name, string password)
        {
            string query = $"INSERT INTO passwords (NickName, Password) VALUES('{name}', '{password}');";
            int number = -1;
            using (var connection = GetDbConnection())
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    number = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return number;
        }

        public int deleteUser(string name)
        {
            string query = $"Delete FROM passwords WHERE NickName = '{name}'";
            int number = -1;
            using (var connection = GetDbConnection())
            {
                try
                {
                    connection.Open();
                    var cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Connection = connection;
                    number = cmd.ExecuteNonQuery();
                    Console.WriteLine("Успішно видалено: {0}", number);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return number;
        }

        public List<string> GetAllUsers(int id)
        {
            int ID = -1;

            List<string> users = new List<string>();
            string query = $"SELECT * FROM passwords";

            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        ID = Int32.Parse(row["id"].ToString());
                        if (ID != id)
                        {
                            users.Add(row["NickName"].ToString());
                        }
                    }
                }
                reader.Close();
            }
            return users;
        }
    }
}
