using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyClient.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для AnimatedLoadingBlock.xaml
    /// </summary>
    /// 
    public partial class AnimatedLoadingBlock : UserControl
    {

        //public Storyboard Storyboard { get; set; }

        public AnimatedLoadingBlock()
        {
            InitializeComponent();
            //Storyboard = Storyboad
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
