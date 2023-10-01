using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfMessengerLibrary
{
    [ServiceContract(CallbackContract = typeof(IServiceMessagerCallback))]
    public interface IServiceMessager
    {
        [OperationContract]
        long getConnect(string login);

        [OperationContract]
        void Disconnect(long id);

        [OperationContract(IsOneWay = true)]
        void SendMessage(MessageServer ms, string nick);
    }

    public interface IServiceMessagerCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendMessageCallback(MessageServer m);
    }

    [DataContract]
    public class MessageServer
    {
        private int id;
        private string userFrom;
        private string userTo;
        private DateTime date;
        private string textMessage;

        public MessageServer()
        {
            date = new DateTime();
        }
        public MessageServer(int _id, string _userFrom, string _userTo, DateTime _date, string _textMessage)
        {
            id = _id;
            userFrom = _userFrom;
            userTo = _userTo;
            date = _date;
            textMessage = _textMessage;
        }

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public string UserFrom
        {
            get { return userFrom; }
            set { userFrom = value; }
        }
        [DataMember]
        public string UserTo
        {
            get { return userTo; }
            set { userTo = value; }
        }
        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember]
        public string TextMessage
        {
            get { return textMessage; }
            set { textMessage = value; }
        }
    }
}
