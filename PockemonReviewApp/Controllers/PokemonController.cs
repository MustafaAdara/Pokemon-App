using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PockemonReviewApp.Dto;
using PockemonReviewApp.Interfaces;
using PockemonReviewApp.Models;
using PockemonReviewApp.Repository;

namespace PockemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private object _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IReviewerRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _pokemonRepository.GetPokemonById(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/name")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(string name1)
        {
            if (!_pokemonRepository.PokemonExisting(name1))
                return NotFound();

            var name = _pokemonRepository.GetPokemonByName(name1);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(name);

        }


        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId,[FromBody] PokemonDto createPokemon)
        {
            if (createPokemon == null) return BadRequest(ModelState);


            var existingPokemon = _pokemonRepository.GetPokemons()
                .Where(c => c.Name.Trim() == createPokemon.Name.TrimEnd()).FirstOrDefault();

            if (existingPokemon != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(createPokemon);

            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while doin");
                return StatusCode(500, ModelState);
            }

            return Ok(pokemonMap);
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokemonId, [FromBody] PokemonDto updatePokemon)
        {

            if (updatePokemon == null) return BadRequest(ModelState);
            if (pokemonId == 0) return BadRequest(ModelState);
            if (pokemonId != updatePokemon.Id) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (!_pokemonRepository.PokemonExists(pokemonId)) return NotFound();

            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "somthing went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(pokemonMap);
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }


            var pokemonToDelete = _pokemonRepository.GetPokemonById(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}
    

