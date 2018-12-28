using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyClient.Models
{
    public class NoteModel
    {
        public int NoteID { get; set; }

        public string Headline { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }

        public NoteModel()
        {
            Headline = string.Empty;
            Description = string.Empty;
            Date = string.Empty;
        }

        public string[] GetDate ()
        {
            return Date.Split('.');
        }
    }
}
