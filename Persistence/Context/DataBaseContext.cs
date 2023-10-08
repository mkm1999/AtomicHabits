using Application.Interfaces.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class DataBaseContext : DbContext , IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Name = "Admin",
                Id = 1
            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Name = "User",
                Id = 2
            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 3,
                Name = "Creator",
            });
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        }

        //DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
    }
}
