using Client.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Client.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private string pass;
        private string nick;

        private SrvRefinfo.ServiceInfoClient srv;

        public MainWindowVM() { }

        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                NotifyPropertyChanged("Nick");
            }
        }
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                NotifyPropertyChanged("Pass");
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

        private SimpleCommand checkCommand;
        public SimpleCommand CheckCommand
        {
            get
            {
                return checkCommand ??
                  (checkCommand = new SimpleCommand(obj =>
                  {
                      try
                      {
                          if (Nick.Length > 0 && Pass.Length > 0)
                          {
                              userCheck();
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  },
                  (obj) => Nick != null && Pass != null));
            }
        }
        private SimpleCommand registrationCommand;
        public SimpleCommand RegistrationCommand
        {
            get
            {
                return registrationCommand ??
                  (registrationCommand = new SimpleCommand(obj =>
                  {
                      makeRegistrationWindow();                    
                  },
                  (obj) => Nick != null && Pass != null));
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

        public void userCheck()
        {
            using (var srv = new SrvRefinfo.ServiceInfoClient())
            {               
                if (!nickCheck())
                {
                    MessageBox.Show("User don`t found");
                    return;
                }
                if (!passCheck())
                {
                    MessageBox.Show("Incorrect password");
                    return;
                }

                string p = GetHash(Pass);
                int id = srv.getUserId(Nick, p);
                string name = Nick;
                makeDialogWindow(id, name);
            }
        }

        private bool nickCheck()
        {
            bool result = false;

            using (var srv = new SrvRefinfo.ServiceInfoClient())
            {
                result = srv.isLogExist(Nick);
            }

            return result;
        }

        private bool passCheck()
        {
            bool result = true;
            string p = GetHash(Pass);
            using (var srv = new SrvRefinfo.ServiceInfoClient())
            {
                result = srv.isPassExist(Nick, p);
            }
            return result;
        }

        private bool testLogin()
        {
            if (Nick == null)
                return false;
            string test = Nick.Replace(" ", "");
            if (test != Nick)
                return false;
            if (Nick.Length < 3 || Nick.Length > 20)
                return false;

            return true;
        }
        private bool testPass()
        {
            if (Pass == null)
                return false;
            string test = Pass.Replace(" ", "");
            if (test != Pass)
                return false;
            if (Pass.Length < 6 || Pass.Length > 20)
                return false;

            return true;
        }       
        private bool checkLogin()
        {
            bool result = true;
            using (srv = new SrvRefinfo.ServiceInfoClient())
            {
                result = srv.isLogExist(Nick);
            }
            return result;
        }

        private void makeAboutWindow()
        {
            MessageBox.Show("For log into the application, enter your username and password.\n" +
                "For register, create a nickname and password.\n" +
                "If there is a nickname in the database, the program will report this.");
        }

        private void makeDialogWindow(int ID, string Nick)
        {
            MessengerWindow mainWindow = new MessengerWindow(ID, Nick);
            mainWindow.Show();
            Application.Current.Windows[0].Close();
        }

        private void makeRegistrationWindow()
        {
            if (!testLogin())
            {
                MessageBox.Show("Login must be between 3 and 20 characters");
                return;
            }
            if (!testPass())
            {
                MessageBox.Show("Password must be between 6 and 30 characters");
                return;
            }

            if (checkLogin())
            {                
                MessageBox.Show("Login is exist");
                return;
            }

            int result = -1;
            string password = GetHash(Pass);
            using (srv = new SrvRefinfo.ServiceInfoClient())
            {
                result = srv.Registration(Nick, password);
            }
            if (result == -1)
            {
                MessageBox.Show("Registration has failed");
            }
            else
            {
                try
                {
                    srv = new SrvRefinfo.ServiceInfoClient();
                    int id = srv.getUserId(Nick, password);
                    makeDialogWindow(id, Nick);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void closeProgram()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private static string GetHash(string input)
        {
            //SHA512
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
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
