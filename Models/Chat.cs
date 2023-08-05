using SQLite;

namespace ChatApp.Models
{
    [Table("Chat")]
    public class Chat
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ChatName { get; set; }
        public int ChatLastMessage { get; set; }
        public DateTime ChatLastDate { get; set; }
    }
}
