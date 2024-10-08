namespace CapstoneDAL.Models.Dtos
{
    public class MessageDto
    {
       

        public string Content { get; set; }

        public byte[]? Attachment { get; set; }

        public string? AttachmentType { get; set; }

        public string? AttachmentName { get; set; }

        public string? TicketId { get; set; }

        // Default constructor
        public MessageDto()
        {
        }

        // Constructor without ID (useful for creating new instances)
        public MessageDto(string content, byte[]? attachment, string? attachmentType, string? attachmentName, string ticketId)
        {
            Content = content;
            Attachment = attachment;
            AttachmentType = attachmentType;
            AttachmentName = attachmentName;
            TicketId = ticketId;
        }

       
    }
}
