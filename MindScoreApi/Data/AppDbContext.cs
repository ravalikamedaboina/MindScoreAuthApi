using Microsoft.EntityFrameworkCore;
using MindScoreApi.Models;
using System.Collections.Generic;

namespace MindScoreApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}