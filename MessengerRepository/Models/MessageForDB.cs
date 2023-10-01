using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerRepository.Models
{
    public class MessageForDB
    {
        private int id;
        private string userFrom;
        private string userTo;
        private long date;
        private string textMessage;

        public MessageForDB() { }
        public MessageForDB(int _id, string _userFrom, string _userTo, long _date, string _textMessage)
        {
            id = _id;
            userFrom = _userFrom;
            userTo = _userTo;
            date = _date;
            textMessage = _textMessage;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string UserFrom
        {
            get { return userFrom; }
            set { userFrom = value; }
        }
        public string UserTo
        {
            get { return userTo; }
            set { userTo = value; }
        }
        public long Date
        {
            get { return date; }
            set { date = value; }
        }
        public string TextMessage
        {
            get { return textMessage; }
            set { textMessage = value; }
        }
    }
}
