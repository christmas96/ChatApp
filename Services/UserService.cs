using ChatApp.Interfaces;
using ChatApp.Models;
using System.Text.Json;

namespace ChatApp.Services
{
    public class UserService : IUserService
    {
        private User _currentUser;

        public async Task<User> GetUser()
        {
            if(_currentUser is null)
            {
                var user = await SecureStorage.GetAsync("user");

                if (!string.IsNullOrEmpty(user))
                {
                    _currentUser = JsonSerializer.Deserialize<User>(user);
                }
            }

            return _currentUser;
        }

        public async Task SaveUser(User user)
        {
            await SecureStorage.SetAsync("user", JsonSerializer.Serialize(user));
        }
    }
}
