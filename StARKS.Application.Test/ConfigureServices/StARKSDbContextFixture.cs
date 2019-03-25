using Microsoft.EntityFrameworkCore;
using StARKS.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StARKS.Application.Test.Services
{
    public class StARKSDbContextFixture : IDisposable
    {
        public StARKSDbContextFixture()
        {
            var builder = new DbContextOptionsBuilder<StARKSDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            this.Instance = new StARKSDbContext(builder.Options);
            this.Instance.Database.EnsureCreated();
        }

        public StARKSDbContext Instance { get; set; }

        public void Dispose()
        {
            this.Instance.Dispose();
        }
    }
}
