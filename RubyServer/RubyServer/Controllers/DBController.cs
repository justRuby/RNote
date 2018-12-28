using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

using RubyServer.Models;
using SQLite;

namespace RubyServer.Controllers
{
    public class DBController
    {
        private static string databaseName = "rubyDB.db";
        private static string folderPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string databasePath = Path.Combine(folderPath, databaseName);

        private static DBController _instance;
        private static object syncRoot = new Object();

        private DBController() { }

        public static DBController GetInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new DBController();
                }
            }
            return _instance;
        }

        public async Task<List<NoteModel>> GetNotesAsync()
        {
            List<NoteModel> content = null;

            using (SQLiteConnection con = new SQLiteConnection(databasePath))
            {
                var result = con.Table<NoteModel>().ToList();

                content = result;
            }

            return await Task.FromResult(content);
        }

        public async Task<bool> UpdateNoteAsync(NoteModel note)
        {
            bool result = false;

            using (SQLiteConnection con = new SQLiteConnection(databasePath))
            {
                con.Update(note, typeof(NoteModel));
                result = con.Table<NoteModel>().Contains(note);
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> AddNoteAsync(NoteModel note)
        {
            bool result = false;

            using (SQLiteConnection con = new SQLiteConnection(databasePath))
            {
                con.Insert(note, typeof(NoteModel));
                result = con.Table<NoteModel>().Contains(note);
            }

            return await Task.FromResult(result);
        }
    }
}
