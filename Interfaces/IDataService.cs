using ChatApp.Models;

namespace ChatApp.Interfaces
{
    public interface IDataService
    {
        Task<IList<Chat>> GetChats();
        Task<IList<Message>> GetMessages(long chatId);
        Task SaveChat(Chat chat);
        Task SaveMessage(Message message);
        int ChatsCount();
    }
}
