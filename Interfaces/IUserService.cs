using ChatApp.Models;

namespace ChatApp.Interfaces
{
    public interface IUserService
    {
        User GetUser();
        void SaveUser(User user);
    }
}
