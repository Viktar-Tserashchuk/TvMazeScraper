using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.DataAccess
{
    public class TvShowsContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<Actor> Actors { get; set; }

        public TvShowsContext(string connectionString) : base (connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Show>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder
                .Entity<Actor>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder
                .Entity<Show>()
                .HasMany(show => show.Actors)
                .WithMany(actor => actor.Shows)
                .Map(cs =>
                {
                    cs.MapLeftKey("ShowId");
                    cs.MapRightKey("ActorId");
                    cs.ToTable("ShowActor");
                });
        }
    }
}
