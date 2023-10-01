using Client.Commands;
using Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Client.ViewModel
{
    public class MessengerWindowVM : INotifyPropertyChanged, SrvRefM.IServiceMessagerCallback
    {
        private UserOfMsngr currentUser;
        private Contact selectedContact;
        private ObservableCollection<Contact> userContacts;
        private UsersMessagesVM usersMessagesVM;

        private long serviceId;
        private SrvRefM.ServiceMessagerClient srvRefM;

        public MessengerWindowVM(int ID, string Nick)
        {
            currentUser = new UserOfMsngr();
            currentUser.ID = ID;
            currentUser.NickName = Nick;

            userContacts = getContacts(ID);

            serviceId = -1;
            try
            {
                srvRefM = new SrvRefM.ServiceMessagerClient(new System.ServiceModel.InstanceContext(this));
                serviceId = srvRefM.getConnect(currentUser.NickName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public UsersMessagesVM UsersMessages
        {
            get { return usersMessagesVM; }
            set
            {
                usersMessagesVM = value;
                NotifyPropertyChanged("UsersMessages");
            }
        }
        public UserOfMsngr CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                NotifyPropertyChanged("CurrentUser");
            }
        }
        public Contact SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                NotifyPropertyChanged("SelectedContact");
            }
        }

        public ObservableCollection<Contact> UserContacts
        {
            get { return userContacts; }
            set
            {
                userContacts = value;
                NotifyPropertyChanged("UserContacts");
            }
        }

        private SimpleCommand contactCommand;
        public SimpleCommand ContactCommand
        {
            get
            {
                return contactCommand ??
                  (contactCommand = new SimpleCommand(obj =>
                  {
                      UsersMessages = new UsersMessagesVM(currentUser.NickName, SelectedContact.Nick);
                      
                      Contact user = UserContacts.FirstOrDefault(u => u.Nick == SelectedContact.Nick);             
                      try
                      {
                          user.getLetter();
                      }
                      catch (Exception ex) 
                      { 
                          MessageBox.Show(ex.Message); 
                      };
                  }));
            }
        }
        private SimpleCommand sendCommand;
        public SimpleCommand SendCommand
        {
            get
            {
                return sendCommand ??
                  (sendCommand = new SimpleCommand(obj =>
                  {
                      sendMessage();
                  },
                  (obj) => UsersMessages != null));
            }
        }
        private SimpleCommand logCommand;
        public SimpleCommand LogCommand
        {
            get
            {
                return logCommand ??
                  (logCommand = new SimpleCommand(obj =>
                  {
                      makeLogWindow();
                  }));
            }
        }
        private SimpleCommand aboutCommand;
        public SimpleCommand AboutCommand
        {
            get
            {
                return aboutCommand ??
                  (aboutCommand = new SimpleCommand(obj =>
                  {
                      makeAboutWindow();
                  }));
            }
        }
        private SimpleCommand exitCommand;
        public SimpleCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                  (exitCommand = new SimpleCommand(obj =>
                  {
                      closeProgram();
                  }));
            }
        }

        private ObservableCollection<Contact> getContacts(int ID)
        {
            ObservableCollection<Contact> newContacts = new ObservableCollection<Contact>();
            List<string> contactNames = new List<string>();
            SrvRefinfo.ServiceInfoClient srv = null;

            try
            {
                srv = new SrvRefinfo.ServiceInfoClient();
                contactNames = srv.getAllUsers(ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (srv != null)
                {
                    srv.Close();
                }
            }

            if (contactNames.Count > 0)
            {
                foreach (var item in contactNames)
                {
                    newContacts.Add(
                        new Contact(item));
                }
            }
            return newContacts;
        }

        private void sendMessage()
        {
            int count = UsersMessages.listOfMessages.Count;

            try
            {
                if (UsersMessages != null)
                {
                    string txt;
                    if (UsersMessages.TextMessage != null)
                    {
                        txt = UsersMessages.TextMessage;
                        Client.Model.Message m = new Client.Model.Message()
                        {
                            ID = count + 1,
                            UserFrom = CurrentUser.NickName,
                            UserTo = UsersMessages.SelectedContactNick,
                            Date = DateTime.Now,
                            TextMessage = txt
                        };
                        UsersMessages.listOfMessages.Add(m);
                        UsersMessages.TextMessage = "";
                        try
                        {
                            SrvRefM.MessageServer ms = new SrvRefM.MessageServer()
                            {
                                ID = count + 1,
                                UserFrom = m.UserFrom,
                                UserTo = m.UserTo,
                                Date = m.Date,
                                TextMessage = m.TextMessage
                            };
                            SrvRefM.ServiceMessagerClient srvRefM = new SrvRefM.ServiceMessagerClient(new System.ServiceModel.InstanceContext(this));
                            srvRefM.SendMessage(ms, UsersMessages.SelectedContactNick);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void closingPrepare()
        {
            if (serviceId != -1)
            {
                try
                {
                    srvRefM.Disconnect(serviceId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
 
        private void makeAboutWindow()
        {
            MessageBox.Show($"Welcome, {currentUser.NickName}.\nA list of all users is on the right side.\n" +
                "For to start a tallking, select a nickname on the list and double-click on it.\n" +
                "If you receive a message from other user, the user's picture will change to the letter.");
        }
        private void makeLogWindow()
        {
            closingPrepare();
            MainWindow logWindow = new MainWindow();
            logWindow.Show();
            Application.Current.Windows[0].Close();
        }
        private void closeProgram()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void SendMessageCallback(SrvRefM.MessageServer ms)
        {
            try
            {
                if (UsersMessages != null)
                {
                    if (ms.UserFrom == UsersMessages.SelectedContactNick)
                    {
                        UsersMessages.listOfMessages.Add(new Client.Model.Message()
                        {
                            UserFrom = ms.UserFrom,
                            UserTo = ms.UserTo,
                            Date = ms.Date,
                            TextMessage = ms.TextMessage
                        });
                    }
                    else
                    {
                        changeStatusContact(ms.UserFrom);
                    }
                }
                if (UsersMessages == null)
                {
                    changeStatusContact(ms.UserFrom);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void changeStatusContact(string nick)
        {
            Contact user = UserContacts.FirstOrDefault(u => u.Nick == nick);

            if (user != null)
            {
                try
                {
                    user.sendLetter();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        ~MessengerWindowVM()
        {
            if (srvRefM != null)
            {
                srvRefM.Disconnect(serviceId);
            }
        }
    }
}
