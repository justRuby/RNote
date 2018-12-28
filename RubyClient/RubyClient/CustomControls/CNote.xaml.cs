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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyClient.CustomControls
{
    using RubyClient.Models;

    public partial class CNote : UserControl
    {
        public NoteModel Note { get; private set; }
        public bool isEmptyNote { get; private set; }

        public CNote()
        {
            InitializeComponent();

            Note = new NoteModel();
            DataContext = Note;

            isEmptyNote = true;
        }

        public CNote(NoteModel note)
        {
            InitializeComponent();

            Note = note;
            DataContext = Note;
        }
    }
}
