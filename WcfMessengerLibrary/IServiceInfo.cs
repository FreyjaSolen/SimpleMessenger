using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WcfMessengerLibrary
{
    [ServiceContract]
    public interface IServiceInfo
    {
        [OperationContract]
        bool isLogExist(string login);

        [OperationContract]
        bool isPassExist(string login, string pass);

        [OperationContract]
        int getUserId(string login, string pass);

        [OperationContract]
        int Registration(string login, string pass);

        [OperationContract]
        List<string> getAllUsers(int id);

        [OperationContract]
        List<MessageServer> getMessage(string userOwner, string userTo);
    }
}
