using MessengerRepository.Models;
using MessengerRepository.Repository;
using System;
using System.Collections.Generic;

namespace WcfMessengerLibrary
{
    public class ServiceInfo : IServiceInfo
    {
        private string nameOfDB = "messenger_local";

        public bool isLogExist(string login)
        {
            bool result = false;

            try
            {
                UserRepository userRepo = new UserRepository(nameOfDB);
                result = userRepo.isLogExist(login);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool isPassExist(string login, string pass)
        {
            bool result = false;            

            try
            {
                UserRepository userRepo = new UserRepository(nameOfDB);
                result = userRepo.isPassExist(login, pass);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public int getUserId(string login, string pass)
        {
            int id = -1;           

            try
            {
                UserRepository userRepo = new UserRepository(nameOfDB);
                id = userRepo.getUserID(login, pass);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return id;
        }

        public int Registration(string login, string pass)
        {            
            UserRepository userRepo = new UserRepository(nameOfDB);

            int result = userRepo.addUser(login, pass);

            return result;
        }            

        public List<string> getAllUsers(int id)
        {
            List<string> users = new List<string>();
            UserRepository userRepo = new UserRepository(nameOfDB);

            try
            {
                users = userRepo.GetAllUsers(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return users;
        }

        public List<MessageServer> getMessage(string userOwner, string userTo)
        {
            List<MessageServer> messageServers = new List<MessageServer>();
            MessageRepository m = new MessageRepository(nameOfDB);

            try
            {
                List<MessageForDB> messages = m.getUsersMessage(userOwner, userTo);
                foreach (var item in messages)
                {
                    messageServers.Add(new MessageServer()
                    {
                        ID = item.ID,
                        UserFrom = item.UserFrom,
                        UserTo = item.UserTo,
                        Date = new DateTime(item.Date),
                        TextMessage = item.TextMessage
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return messageServers;
        }              
    }
}
