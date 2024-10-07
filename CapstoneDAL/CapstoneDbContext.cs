using Capstone.Models;
using Capstone.Models.Entities;
using CapstoneDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneDAL
{
	public class CapstoneDbContext : DbContext
	{
		public CapstoneDbContext(DbContextOptions<CapstoneDbContext> options) : base(options) { }

		public DbSet<UserDetails> Users { get; set; }
		public DbSet<DisplayAgent> DisplayAgents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<UserDetails>().ToTable("Users");
			modelBuilder.Entity<DisplayAgent>()
		  .ToTable("display_agents");

			modelBuilder.Entity<DisplayAgent>()
				.HasKey(e => e.Id);  // Set Id as Primary Key

			modelBuilder.Entity<DisplayAgent>()
				.Property(e => e.Id)
				.ValueGeneratedNever();

            modelBuilder.Entity<Ticket>()
               .HasOne(t => t.AssignedAgentEntity) // A Ticket has one AssignedAgent
               .WithMany(da => da.Tickets) // Each DisplayAgent has many Tickets
               .HasForeignKey(t => t.AgentId) // Foreign key in the Ticket table
               .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

        }
    }
}
