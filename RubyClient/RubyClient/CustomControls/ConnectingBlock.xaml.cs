using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyClient.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для ConnectingBlock.xaml
    /// </summary>
    public partial class ConnectingBlock : UserControl, INotifyPropertyChanged
    {
        private string _textString;
        public string TextString
        {
            get
            {
                return _textString;
            }
            set
            {
                _textString = value;
                NotifyPropertyChanged("TextString");
            }
        }

        public ConnectingBlock()
        {
            InitializeComponent();

            ConnectionStatusTextBlock.DataContext = this;
        }

        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
