using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyServer.Models
{
    public class NoteModel
    {
        [PrimaryKey, AutoIncrement]
        public int NoteID { get; set; }

        public string Headline { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
    }
}
