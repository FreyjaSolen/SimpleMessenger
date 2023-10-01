using MessengerRepository.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace MessengerRepository.Repository
{
    public class MessageRepository
    {
        string ConnectionName { get; set; }

        public MessageRepository(string connectionName)
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

        public int addMessage(string from, string to, string message)
        {
            DateTime tm = new DateTime();
            tm = DateTime.Now;
            long Date = tm.Ticks;
            string query = $"INSERT INTO messages (UserFrom, UserTo, Date, TextMessage) VALUES ((SELECT id FROM passwords WHERE NickName = '{from}'), " +
                $"(SELECT id FROM passwords WHERE NickName = '{to}'), '{Date}', '{message}');";

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

        public List<MessageForDB> getUsersMessage(string userOwner, string userTo)
        {
            string query = $"SELECT m.id, p1.NickName as p1, p2.NickName as p2, Date, TextMessage FROM messages m " +
                $"INNER JOIN passwords p1 ON m.UserFrom = p1.id " +
                $"INNER JOIN passwords p2 ON m.UserTo = p2.id " +
                $"WHERE m.UserFrom = (SELECT id FROM passwords WHERE NickName = '{userOwner}') " +
                $"AND m.UserTo = (SELECT id FROM passwords WHERE NickName = '{userTo}') " +
                $"OR m.UserTo  = (SELECT id FROM passwords WHERE NickName = '{userOwner}') " +
                $"AND m.UserFrom = (SELECT id FROM passwords WHERE NickName = '{userTo}');";

            List<MessageForDB> messages = null;
            using (var connection = GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();

                messages = new List<MessageForDB>();
                if (reader.HasRows)
                {
                    foreach (DbDataRecord row in reader)
                    {
                        try
                        {
                            messages.Add(new MessageForDB()
                            {
                                ID = int.Parse(row["id"].ToString()),
                                UserFrom = row["p1"].ToString(),
                                UserTo = row["p2"].ToString(),
                                Date = long.Parse((row["Date"].ToString())),
                                TextMessage = row["TextMessage"].ToString(),
                            });
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("***NO MESSAGE***");
                }
                reader.Close();
            }
            return messages;
        }
    }
}
