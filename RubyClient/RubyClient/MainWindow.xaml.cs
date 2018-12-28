using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyClient
{
    using RubyClient.Controller;
    using RubyClient.Models;
    using RubyClient.CustomControls;
    using RubyClient.CustomExstension;

    public partial class MainWindow : Window
    {
        private RAController restApiController;
        public List<NoteModel> ListNoteModels { get; private set; }

        private ConnectingBlock cBlock;
        private List<CNote> cNotes = new List<CNote>();
        private NoteModel _current;

        private DispatcherTimer _reconnectingTimer;
        private int countSeconds;
        private int currentSeconds;

        private bool isSearching;
        private bool isNewNote;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _reconnectingTimer = new DispatcherTimer();
            _reconnectingTimer.Interval = TimeSpan.FromSeconds(1);
            _reconnectingTimer.Tick += ReconnectToServer;

            countSeconds = currentSeconds = 10;

            AddConnectionBlock();

            restApiController = RAController.GetInstance();
            await InitializeNotesAsync();
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListNoteModels == null)
                return;

            if (!isSearching)
            {
                isSearching = true;

                ClearNotes();
                string text = (sender as TextBox).Text;

                if (text != "")
                {
                    await ViewNotes(ListNoteModels.SearchNotes(text));
                }
                else
                {
                    await ViewNotes(ListNoteModels);
                }

                isSearching = false;
            }

        }

        private void AddNewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            isNewNote = true;

            _current = new NoteModel
            {
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };

            SelectedNoteGrid.DataContext = _current;
            ChangeVisibility();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in cNotes)
            {
                if(item.Button == (sender as Button))
                {
                    _current = item.Note;
                    break;
                }
            }
            
            SelectedNoteGrid.DataContext = _current;

            ChangeVisibility();
        }

        private async void ReturnToListButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisibility();

            try
            {
                if(isNewNote)
                {
                    await restApiController.AddNewNoteAsync(_current);

                    isNewNote = false;
                    ClearNotes();

                    await InitializeNotesAsync();
                       
                } 
                else
                    await restApiController.UpdateDataNoteAsync(_current);
                
                _current = null;
                SelectedNoteGrid.DataContext = null;
            }
            catch (Exception)
            {
                MessageBox.Show("505 ошибка сервера!");
            }

        }

        private async void ReconnectToServer(object sender, EventArgs e)
        {
            if(currentSeconds >= 0)
            {
                cBlock.TextString = "Connection lost - retrying " + currentSeconds.ToString() + " sec";
                currentSeconds--;
            }
            else
            {
                if (countSeconds <= 60)
                    countSeconds += 10;

                currentSeconds = countSeconds;
                _reconnectingTimer.Stop();

                await InitializeNotesAsync();
            }
        }

        private async Task InitializeNotesAsync()
        {
            try
            {

                if(cBlock == null)
                {
                    AddConnectionBlock();
                }

                cBlock.TextString = "Connecting..";
                await restApiController.CheckConnective();
            }
            catch (Exception)
            {
                _reconnectingTimer.Start();
                return;
            }

            cBlock.TextString = "Loading..";

            ListNoteModels = await restApiController.GetNotesAsync();

            if (ListNoteModels != null)
            {
                await ViewNotes(ListNoteModels);
            }
            else
            {
                NotHaveNotesForViewGrid.ChangeVisibility(TypeObjects.Grid);
            }

            RemoveConnectionBlock();
        }

        private async Task<bool> ViewNotes(List<NoteModel> listNotes, NoteModel nNote = null)
        {
            if (nNote != null)
                listNotes.Add(nNote);

            foreach (var note in listNotes)
            {
                var temp = new CNote(note);
                temp.Button.Click += Button_Click;
                cNotes.Add(temp);
            }

            cNotes.Reverse();

            foreach (var note in cNotes)
            {
                RightMiddleWrapPanel.Children.Add(note);
            }

            return await Task.FromResult(true);
        }

        private void ClearNotes()
        {
            foreach (var item in RightMiddleWrapPanel.Children)
                (item as CNote).Button.Click -= Button_Click;

            cNotes.Clear();
            RightMiddleWrapPanel.Children.Clear();
        }

        private void AddConnectionBlock()
        {
            cBlock = new ConnectingBlock();
            ContentGrid.Children.Add(cBlock);
        }

        private void RemoveConnectionBlock()
        {
            ContentGrid.Children.Remove(cBlock);
            cBlock = null;
        }

        private void ChangeVisibility()
        {
            SelectedNoteGrid.ChangeVisibility(TypeObjects.Grid);
            scrollViewer.ChangeVisibility(TypeObjects.ScrollViewer);
            AddNewNoteButton.ChangeVisibility(TypeObjects.Button);
        }

        private async void GetDateButton_Click(object sender, RoutedEventArgs e)
        {
            await InitializeNotesAsync();
        }

    }
}
