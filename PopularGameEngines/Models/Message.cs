using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopularGameEngines.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public AppUser From { get; set; } = new();

        public DateOnly Date { get; set; }

        public int Rating { get; set; }

        [ForeignKey("OriginalMessageId")]
        public List<Message> Replies { get; set; } = new();

        public int? OriginalMessageId { get; set; } = null;
    }
}
