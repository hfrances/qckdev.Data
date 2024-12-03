using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qckdev.DataTest
{
    sealed class TestDbContext : DbContext
    {

        public DbSet<Entity> Entities => Set<Entity>();

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
