using ChatApp.Interfaces;
using ChatApp.Models;
using SQLite;
using System.Diagnostics;

namespace ChatApp.Services
{
    public class DataService : IDataService
    {
        const string _databaseFilename = "ChatAppSQLite.db3";
        string _databasePath => Path.Combine(FileSystem.AppDataDirectory, _databaseFilename);

        SQLiteAsyncConnection _database;

        public DataService()
        {
            _database = new SQLiteAsyncConnection(_databasePath);

            _database.CreateTableAsync<User>();
            _database.CreateTableAsync<Chat>();
            _database.CreateTableAsync<Message>();

            _database.DeleteAllAsync<Chat>();
            _database.DeleteAllAsync<Message>();
        }

        public async Task<IList<Chat>> GetChats()
        {
            try
            {
                return await _database.Table<Chat>()?.ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new List<Chat>();
        }

        public async Task<IList<Message>> GetMessages(long chatId)
        {
            try
            {
                return await _database.Table<Message>()?.Where(c => c.ChatId == chatId)?.ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new List<Message>();
        }

        public async Task SaveChat(Chat chat)
        {
            await _database.InsertAsync(chat);
        }

        public async Task SaveMessage(Message message)
        {
            await _database.InsertAsync(message);
        }

        public int ChatsCount()
        {
            return _database.Table<Chat>().CountAsync().GetAwaiter().GetResult();
        }
    }
}
