using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Context
{
    public interface IDataBaseContext
    {
        public int SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

        //DbSets
        DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Report> Reports { get; set; }




    }
}
