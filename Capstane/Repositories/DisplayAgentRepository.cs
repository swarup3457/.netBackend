using System;
using Capstone.Models;
using CapstoneDAL;

namespace TicketManagement.Repositories
{
    public class DisplayAgentRepository : IDisplayAgentRepository
    {
        private readonly CapstoneDbContext _context;

        public DisplayAgentRepository(CapstoneDbContext context)
        {
            _context = context;
        }

        public DisplayAgent FindByEmail(string email)
        {
            return _context.DisplayAgents.FirstOrDefault(da => da.Email == email);
        }

        public IEnumerable<DisplayAgent> GetAll()
        {
            return _context.DisplayAgents.ToList();
        }

        public DisplayAgent GetById(long id)
        {
            return _context.DisplayAgents.Find(id);
        }

        public void Add(DisplayAgent displayAgent)
        {
            _context.DisplayAgents.Add(displayAgent);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
