using MessengerRepository.Repository;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfMessengerLibrary.LogicUsersOfService;

namespace WcfMessengerLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceMessager : IServiceMessager
    {
        private ListUsers listUsers = new ListUsers();
        private string nameOfDB = "messenger_local";

        public long getConnect(string login)
        {
            long id = -1;
            id = listUsers.addUser(login);
            Console.WriteLine($"The user with id - {id} and login {login} has connected");

            return id;
        }

        public void Disconnect(long id)
        {
            listUsers.deleteUserService(id);
            Console.WriteLine($"The user with id - {id} disconnected");
        }

        async public void SendMessage(MessageServer ms, string nick)
        {
            await Task.Run(() =>
            {
                UserService user = listUsers.users.FirstOrDefault(u => u.UserName == nick);

                if (user != null)
                {
                    try
                    {
                        user.Context.GetCallbackChannel<IServiceMessagerCallback>().SendMessageCallback(ms);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); };
                }

                MessageRepository mRepo = new MessageRepository(nameOfDB);
                int r = mRepo.addMessage(ms.UserFrom, ms.UserTo, ms.TextMessage);
                if (r == -1)
                {
                    Console.WriteLine("Error adding message");
                }
            });
        }

        public IServiceMessagerCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IServiceMessagerCallback>();
            }
        }
    }
}
