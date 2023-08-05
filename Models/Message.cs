using ChatApp.Interfaces;
using SQLite;

namespace ChatApp.Models
{
    [Table("Message")]
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public bool MyMessage => UserId == (App.Current.Services.GetService<IUserService>().GetUser().GetAwaiter().GetResult()).Id;
    }
}
