namespace ChatApp.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public string Context { get; set; } 
        public DateTime Date { get; set; }


        public int FromId { get; set; } 
        public int ToId { get; set; } 

        public User FromUser { get; set; }
        public User ToUser { get; set; }
    }
}
