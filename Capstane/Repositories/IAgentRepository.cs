using Capstone.Models;
using CapstoneDAL.Models;

namespace TicketManagement.Repositories
{
    public interface IAgentRepository
    {
        UserDetails FindByEmail(string email);
        // Other methods as needed
    }

    public interface IDisplayAgentRepository
    {
        DisplayAgent FindByEmail(string email);
        IEnumerable<DisplayAgent> GetAll();
        DisplayAgent GetById(long id);
        void Add(DisplayAgent displayAgent);
        void Save();
    }
}
