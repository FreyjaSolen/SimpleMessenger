using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Model
{
    public class Message : INotifyPropertyChanged
    {
        private int id;
        private string userFrom;
        private string userTo;
        private DateTime date;
        private string textMessage;

        public Message()
        {
            date = new DateTime();
        }
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
            }
        }
        public string UserFrom
        {
            get { return userFrom; }
            set
            {
                userFrom = value;
                OnPropertyChanged("UserFrom");
            }
        }
        public string UserTo
        {
            get { return userTo; }
            set { userTo = value; }
        }
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
        public string TextMessage
        {
            get { return textMessage; }
            set
            {
                textMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
