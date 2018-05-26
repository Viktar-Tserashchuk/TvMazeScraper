using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.DataAccess
{
    public class TvShowsContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ShowToActor> ShowToActors { get; set; }

        public TvShowsContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Show>()
                .Property(t => t.Id)
                .ValueGeneratedNever();

            modelBuilder
                .Entity<Actor>()
                .Property(t => t.Id)
                .ValueGeneratedNever();

            modelBuilder
                .Entity<ShowToActor>()
                .HasKey(t => new {t.ShowId, t.ActorId});
        }
    }
}
