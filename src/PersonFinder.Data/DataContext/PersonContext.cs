using Microsoft.EntityFrameworkCore;
using PersonFinder.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonFinder.Data.DataContext
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
