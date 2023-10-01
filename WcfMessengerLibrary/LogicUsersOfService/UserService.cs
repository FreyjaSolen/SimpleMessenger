using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfMessengerLibrary.LogicUsersOfService
{
    public class UserService
    {
        private long id;
        private string userName;
        private OperationContext operationContext;

        public UserService() { }

        public UserService(long _id, string _userName, OperationContext contex)
        {
            id = _id;
            userName = _userName;
            operationContext = contex;
        }

        public long ID
        {
            get { return id; }
            set { id = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public OperationContext Context
        {
            get { return operationContext; }
            set { operationContext = value; }
        }
    }
}
