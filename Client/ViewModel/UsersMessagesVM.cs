using Client.SrvRefinfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Client.ViewModel
{
    public class UsersMessagesVM : INotifyPropertyChanged
    {
        private SrvRefinfo.ServiceInfoClient srvInfo;
        public ObservableCollection<Client.Model.Message> listOfMessages { get; set; }
        private string selectedContactNick;
        private Client.Model.Message selectedMessage;
        private string textMessage;
        private string currentUser;

        public UsersMessagesVM(string userName, string nameContact)
        {
            currentUser = userName;
            SelectedContactNick = nameContact;
            listOfMessages = new ObservableCollection<Client.Model.Message>();
            getMessages();
        }

        public string SelectedContactNick
        {
            get { return selectedContactNick; }
            set
            {
                selectedContactNick = value;
                NotifyPropertyChanged("SelectedContactNick");
            }
        }
        public Client.Model.Message SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                selectedMessage = value;
                NotifyPropertyChanged("SelectedMessage");
            }
        }
        public string TextMessage
        {
            get { return textMessage; }
            set
            {
                textMessage = value;
                NotifyPropertyChanged("TextMessage");
            }
        }

        public void getMessages()
        {
            try
            {
                using (srvInfo = new ServiceInfoClient())
                {
                    List<MessageServer> messageServers = new List<MessageServer>();
                    messageServers = srvInfo.getMessage(currentUser, SelectedContactNick);

                    foreach (var item in messageServers)
                    {
                        listOfMessages.Add(new Client.Model.Message()
                        {
                            ID = item.ID,
                            UserFrom = item.UserFrom,
                            UserTo = item.UserTo,
                            Date = item.Date,
                            TextMessage = item.TextMessage
                        });
                    }
                    srvInfo.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
