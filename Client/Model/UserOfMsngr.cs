using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Model
{
    public class UserOfMsngr : INotifyPropertyChanged
    {
        private int id;
        private string nickName;        

        public UserOfMsngr()
        {
            id = -1;
            nickName = "";
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set
            {
                nickName = value;
                OnPropertyChanged("NickName");
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
