using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RubyClient.CustomExstension
{
    using System.Windows;
    using System.Windows.Controls;

    using RubyClient.Models;

    public enum TypeObjects
    {
        Grid,
        ScrollViewer,
        Button,
        TextBlock
    }

    public static class Exstension
    {
        public static void ChangeVisibility<T>(this T obj, TypeObjects type)
        {

            switch (type)
            {
                case TypeObjects.Button:

                    var button = (obj as Button);
                    button.Visibility = Change(button.Visibility);
                    break;

                case TypeObjects.Grid:

                    var grid = (obj as Grid);
                    grid.Visibility = Change(grid.Visibility);
                    break;

                case TypeObjects.ScrollViewer:

                    var scrollViewer = (obj as ScrollViewer);
                    scrollViewer.Visibility = Change(scrollViewer.Visibility);
                    break;

                case TypeObjects.TextBlock:

                    var textBlock = (obj as TextBlock);
                    textBlock.Visibility = Change(textBlock.Visibility);
                    break;

                default:
                    break;
            }
        }

        private static Visibility Change(Visibility vis)
        {
            return vis == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        public static List<NoteModel> SearchNotes(this List<NoteModel> listNotes, string text)
        {
            List<NoteModel> result = new List<NoteModel>();
            char firstChar = text.First();

            var findedNote = listNotes.AsParallel().Where((x) =>
            {
                var temp = x.Headline;

                if (temp == text)
                    return true;

                var arrTemp = temp.Split(' ');

                foreach (var word in arrTemp)
                {
                    if(word == text)
                    {
                        return true;
                    }
                }

                return false;

            }).FirstOrDefault();

            if(findedNote != null)
            {
                result.Add(findedNote);
                return result;
            }
            else
            {
                var search = listNotes.AsParallel().Where((x) =>
                {
                    var temp = x.Headline;
                    foreach (var chNotes in temp)
                    {
                        foreach (var chText in text)
                        {
                            if (chNotes == chText)
                                return true;
                        }
                    }

                    return false;
                });

                foreach (var item in search)
                {
                    result.Add(item);
                }
            }

            #region Legacy

            //if (char.IsDigit(firstChar))
            //{
            //    var search = listNotes.Where((x) =>
            //    {

            //        if (fullCoincidence)
            //            return false;

            //        foreach (var number in x.GetDate())
            //        {
            //            if(number == text)
            //            {
            //                fullCoincidence = true;
            //                return true;
            //            }

            //            foreach (var chNumber in number)
            //            {
            //                foreach (var chText in text)
            //                {
            //                    if (chNumber == chText)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //        }

            //        return false;
            //    });

            //    foreach (var item in search)
            //    {
            //        result.Add(item);
            //    }
            //}
            //else
            //{
            //    var search = listNotes.Where((x) => 
            //    {
            //        if (fullCoincidence)
            //            return false;

            //        var temp = x.Headline;

            //        if (text == temp)
            //        {
            //            fullCoincidence = true;
            //            return true;
            //        }

            //        foreach (var chNumber in temp)
            //        {
            //            foreach (var chText in text)
            //            {
            //                if (chNumber == chText)
            //                {
            //                    return true;
            //                }
            //            }
            //        }

            //        return false;
            //    });

            //    foreach (var item in search)
            //    {
            //        result.Add(item);
            //    }
            //}
            #endregion

            return result;
        }
    }
}
