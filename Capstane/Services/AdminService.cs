using Capstone.Models;
using Capstone.Models.Entities;
using Capstone.Services;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TicketManagement.Repositories;
using TicketManagement.Services;

namespace Capstane.Services
{
    public class AdminService
    {
        TicketService ticketService;
        DisplayAgentService DisplayAgentServices;
        public AdminService(TicketService ticketService, DisplayAgentService DisplayAgentServices)
        {
            this.ticketService = ticketService;
            this.DisplayAgentServices = DisplayAgentServices;
        }

        public Ticket assignAgentToTicket(String ticketId, long agentId)
        {

            Ticket ticket = ticketService.GetById(ticketId);
                   if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            DisplayAgent agent = DisplayAgentServices.GetDisplayAgentById(agentId);
                  if (agent == null)
            {
                throw new Exception("Agent not found");
            }
            ticket.AssignedAgentEntity = agent;
            ticket.Status = "InProgress";
            ticket.AssignedAgent = agent.Name;

            ticket = ticketService.Updating(ticket);
            return ticket;
        }
    }
}
