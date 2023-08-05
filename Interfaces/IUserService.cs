using ChatApp.Models;

namespace ChatApp.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser();
        Task SaveUser(User user);
    }
}
