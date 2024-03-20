using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PockemonReviewApp.Dto;
using PockemonReviewApp.Interfaces;
using PockemonReviewApp.Models;
using PockemonReviewApp.Repository;
using System.Security.Cryptography.X509Certificates;

namespace PockemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper,
                                IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public ActionResult<Review> Get()
        {
            var reviews = _reviewRepository.GetAllReviews();

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(reviews);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        
        public ActionResult GetReviews(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();

            var review = _reviewRepository.GetReview(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(review);

        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public ActionResult GetReviewsOfAPokemon(int pokeId)
        {
            if (!_reviewRepository.ReviewExists(pokeId))
                return NotFound();

            var reviews = _reviewRepository.GetReviewsOfAPokemon(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId,[FromBody] ReviewDto createReview)
        {
            if (createReview == null) return BadRequest(ModelState);


            var existingReview = _reviewRepository.GetAllReviews()
                .Where(c => c.Title.Trim() == createReview.Title.TrimEnd()).FirstOrDefault();

            if (existingReview != null)
            {
                ModelState.AddModelError("", "Review Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(createReview);

            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon = _pokemonRepository.GetPokemonById(pokeId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while doin");
                return StatusCode(500, ModelState);
            }

            return Ok(reviewMap);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return Ok(reviewMap);
        }
    }
}
