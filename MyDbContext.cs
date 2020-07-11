using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFTest
{
    class MyDbContext : DbContext
    {
        public bool UseLazyLoading { get; }

        public MyDbContext(bool useLazyLoading)
        {
            UseLazyLoading = useLazyLoading;
        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<AbstractChild> AbstractChildren { get; set; }
        public DbSet<ConcreteChild> ConcreteChildren { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(UseLazyLoading);
            
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=EFTest;Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
