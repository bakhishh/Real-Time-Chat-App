using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Entity
{
    public class Announcement
    {
        public int Id { get; set; }

        
        public string Title { get; set; }

        
        public string Content { get; set; } 

        
        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
