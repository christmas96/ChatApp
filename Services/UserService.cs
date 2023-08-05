using ChatApp.Interfaces;
using ChatApp.Models;
using SQLite;
using System.Text.Json;

namespace ChatApp.Services
{
    public class UserService : IUserService
    {
        const string _databaseFilename = "ChatAppSQLite.db3";
        string _databasePath => Path.Combine(FileSystem.AppDataDirectory, _databaseFilename);

        SQLiteConnection _database;
        private User _currentUser;

        public UserService()
        {
            _database = new SQLiteConnection(_databasePath);

            _database.CreateTable<User>();
        }

        public User GetUser()
        {
            if(_currentUser is null)
            {
                _currentUser = _database.Table<User>().Count() > 0 ? _database.Table<User>().First() : null;
            }

            return _currentUser;
        }

        public void SaveUser(User user)
        {
            try
            {
                _database.DeleteAll<User>();
                _database.Insert(user);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
