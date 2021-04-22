using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Pic2Text.Models;

namespace Pic2Text.Data
{
    public class P2TDatabase
    {
        readonly SQLiteAsyncConnection database;

        public P2TDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<P2T>().Wait();
        }

        public Task<List<P2T>> GetElementsAsync()
        {
            //Get all notes.
            return database.Table<P2T>().ToListAsync();
        }

        public Task<P2T> GetNoteAsync(int id)
        {
            // Get a specific note.
            return database.Table<P2T>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveElementAsync(P2T element)
        {
            if (element.ID != 0)
            {
                // Update an existing note.
                return database.UpdateAsync(element);
            }
            else
            {
                // Save a new note.
                return database.InsertAsync(element);
            }
        }

        public Task<int> DeleteElementAsync(P2T element)
        {
            // Delete a note.
            return database.DeleteAsync(element);
        }
    }
}
