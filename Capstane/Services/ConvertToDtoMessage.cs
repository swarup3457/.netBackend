using Capstone.Models.Entities;
using CapstoneDAL.Models;
using CapstoneDAL.Models.Dtos;

namespace HelpDesk16.Services
{
    public class ConvertToDtoMessage
    {
        // Converts MessageTable entity to MessageDto
        public MessageDto ConvertToDto(MessageTable message)
        {
            return new MessageDto
            {
                Content = message.Content,
                Attachment = message.Attachment,
                AttachmentType = message.AttachmentType,
                AttachmentName = message.AttachmentName,
                TicketId = message.Ticket.Id.ToString()  // Assuming Ticket.Id is of type long
            };
        }

        // Converts MessageDto to MessageTable entity
        public MessageTable  ConvertToEntity(MessageDto messageDto)
        {
            var message = new MessageTable
            {
                Content = messageDto.Content,
                Attachment = messageDto.Attachment,
                AttachmentType = messageDto.AttachmentType,
                AttachmentName = messageDto.AttachmentName,
                // The Ticket will need to be assigned later in your business logic
            };

            return message;
        }
    }
}
