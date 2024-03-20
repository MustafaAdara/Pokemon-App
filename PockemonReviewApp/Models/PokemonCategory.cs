namespace PockemonReviewApp.Models
{
    public class PokemonCategory
    {
        public int Pokemonid { get; set; }

        public int Categoryid { get; set; }

        public Pokemon Pockemon { get; set;}

        public Category Category { get; set; } 
    }
}
