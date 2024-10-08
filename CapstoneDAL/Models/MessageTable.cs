using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Models.Entities
{
    [Table("messages")]
    public class MessageTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[]? Attachment { get; set; }

        public string? AttachmentType { get; set; }

        public string? AttachmentName { get; set; }

        // Foreign key for Ticket
        public string TicketId { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }  // Navigation property to Ticket

        public MessageTable() { }

        public MessageTable(string content, byte[]? attachment, string? attachmentType, string? attachmentName, Ticket ticket)
        {
            Content = content;
            Attachment = attachment;
            AttachmentType = attachmentType;
            AttachmentName = attachmentName;
            Ticket = ticket;
        }
    }
}
