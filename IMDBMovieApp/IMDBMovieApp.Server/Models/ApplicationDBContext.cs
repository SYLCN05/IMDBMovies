using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Server.Models
{
    public class ApplicationDBContext:IdentityDbContext<MovieUser, IdentityRole, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserWatchList>()
                .HasKey(uw => new { uw.UserId, uw.MovieId });

            builder.Entity<UserWatchList>()
                .HasOne(uw => uw.User)
                .WithMany()
                .HasForeignKey(uw => uw.UserId);

            builder.Entity<UserWatchList>()
                .HasOne(uw => uw.Movie)
                .WithMany()
                .HasForeignKey(uw => uw.MovieId);

            
        }
        public DbSet<Movie> Movies => Set<Movie>();

        public DbSet<UserWatchList> UserWatchLists => Set<UserWatchList>();
    }
}
