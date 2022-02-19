using Assessment.API.Application.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment.API.Application.DataAccessLayer.Data
{
    public class ApplicationDBContext: DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
         
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(b =>
            {
                b.HasKey(e => e.UniqueId);
                b.Property(e => e.UniqueId).UseIdentityColumn();
            });
        }


        public DbSet<Client> Clients { get; set; }
    }
}