namespace PockemonReviewApp.Models
{
    public class PokemonOwner
    {
        public int Pokemonid { get; set; }
        public int Ownerid { get; set; }
        public Pokemon Pokemon { get; set; } 
        public Owner Owner { get; set; } 
        
    }
}
