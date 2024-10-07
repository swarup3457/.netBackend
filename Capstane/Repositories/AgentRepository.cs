using System.Linq;
using CapstoneDAL;
using CapstoneDAL.Models;

namespace TicketManagement.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly CapstoneDbContext _context;

        public AgentRepository(CapstoneDbContext context)
        {
            _context = context;
        }

        public UserDetails FindByEmail(string email)
        {
            // Find the user by email where the role is 'Agent'
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Role == "Agent");
        }

        // Implement other methods as needed
    }
}
