using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client.Model
{
    public class Contact : INotifyPropertyChanged
    {
        private string nick;
        private BitmapImage pathImg;

        private const string path = "..\\..\\Images\\Flowers.ico";
        private const string letter = "..\\..\\Images\\Letter.ico";

        public Contact() { }
        public Contact(string nick)
        {
            this.nick = nick;
            if (File.Exists(path))
            {
                PathImg = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, path)));
            }
        }

        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                NotifyPropertyChanged("Nick");
            }
        }

        public BitmapImage PathImg
        {
            get { return pathImg; }
            set
            {
                pathImg = value;
                NotifyPropertyChanged("PathImg");
            }
        }

        public void sendLetter()
        {
            try
            {
                if (File.Exists(letter))
                {
                    PathImg = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, letter)));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void getLetter()
        {
            try
            {
                if (File.Exists(path))
                {
                    PathImg = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, path)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        public override string ToString()
        {
            return Nick;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
