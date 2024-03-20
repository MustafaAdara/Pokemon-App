using Microsoft.EntityFrameworkCore;
using PockemonReviewApp.Models;

namespace PockemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonCategory> PokemonCategories  { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<PokemonCategory>()
                    .HasKey(pc => new { pc.Pokemonid, pc.Categoryid });
                modelBuilder.Entity<PokemonCategory>()
                    .HasOne(e => e.Pockemon)
                    .WithMany(f => f.PokemonCategories)
                    .HasForeignKey(p => p.Pokemonid);
                modelBuilder.Entity<PokemonCategory>()
                    .HasOne(e => e.Category)
                    .WithMany(p => p.PokemonCategories)
                    .HasForeignKey(o => o.Categoryid);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(k => new { k.Pokemonid, k.Ownerid });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(k => k.Pokemon)
                .WithMany(p => p.PokemonOwners)
                .HasForeignKey(o => o.Pokemonid);
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(k => k.Owner)
                .WithMany(yi => yi.PokemonOwners)
                .HasForeignKey(o => o.Ownerid);

        }
    }
}
