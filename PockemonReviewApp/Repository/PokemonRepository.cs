using PockemonReviewApp.Data;
using PockemonReviewApp.Interfaces;
using PockemonReviewApp.Models;
using System.Linq;


namespace PockemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public Pokemon GetPokemonById(int id)
        {
            return _context.Pokemon.Where(x => x.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string name)
        {
            return _context.Pokemon.Where(x => x.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

            if (review.Count() <= 0)
                return 0;

            return ( (decimal)review.Sum(r => r.Rating) / review.Count() );
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p=> p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemon.Any(p => p.Id == pokeId);
        }

        public bool PokemonExisting(string name)
        {
            return _context.Pokemon.Any(x => x.Name == name);
        }

        public bool CreatePokemon(int ownerId, int categoryId,Pokemon pokemon)
        {
            var PokeOwner = _context.Owners.Where(x => x.Id == ownerId).FirstOrDefault();
            var PokeCategory =_context.Categories.Where(x => x.Id ==  categoryId).FirstOrDefault();

            PokemonCategory category = new PokemonCategory{
                Category = PokeCategory,
                Pockemon = pokemon
            };
            _context.Add(category);

            PokemonOwner pokemonOwner = new PokemonOwner
            {
                Owner = PokeOwner,
                Pokemon = pokemon

            };
            _context.Add(pokemonOwner);

            _context.Add(pokemon);
            return Save();
        }
        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        public bool Save()
        {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }
    }
}
