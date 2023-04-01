using JWTCreation.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTCreation.SQLInfra
{
    public class JwTDBContext : DbContext
    {
        public JwTDBContext(DbContextOptions<JwTDBContext> options) : base(options) { }
        public DbSet<UserModel> UserCred { get; set; }
        public DbSet<DataModel> UserData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("UserCred");
            modelBuilder.Entity<DataModel>().ToTable("UserData");

        }
    }
}
