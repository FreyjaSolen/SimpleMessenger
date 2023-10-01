using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfMessengerLibrary.LogicUsersOfService
{
    public class ListUsers
    {
        public List<UserService> users { get; set; }

        public ListUsers()
        {
            users = new List<UserService>();
        }

        private void add(long id, string nickName, OperationContext contex)
        {
            users.Add(new UserService(id, nickName, contex));
        }
        private bool checkUser(string nickName)
        {
            bool result = true;          

            UserService user = users.FirstOrDefault(u => u.UserName == nickName);

            if (user != null)
            {
                result = false;
            }

            return result;
        }

        public long addUser(string nickName)
        {
            long id = -1;

            bool result = checkUser(nickName);

            if (result == true)
            {
                UsersCount.setId();
                id = UsersCount.getId();
                add(id, nickName, OperationContext.Current);
            }

            return id;
        }

        public bool deleteUser(long id)
        {
            bool result = false;

            UserService user = users.FirstOrDefault(u => u.ID == id);

            if (user != null)
            {
                users.Remove(user);
                result = true;
            }

            return result;
        }

        public void deleteUserService(long id)
        {
            UserService user = users.FirstOrDefault(u => u.ID == id);
            if (user != null)
            {
                users.Remove(user);
            }
        }
    }
}