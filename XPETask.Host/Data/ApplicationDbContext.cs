using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;
using XPETask.Host.Entities;

namespace XPETask.Host.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
        .HasIndex(c => c.Email)
        .IsUnique();

        }
    }
}
