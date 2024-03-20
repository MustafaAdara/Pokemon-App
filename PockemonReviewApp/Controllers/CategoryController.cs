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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public ActionResult GetAllCategories()
        {
            var categories = _categoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public ActionResult GetCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound();

            var category = _categoryRepository.GetCategory(id);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(category);

        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public ActionResult GetPokemonByCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var pokemons = _categoryRepository.GetPokemonByCategory(categoryId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemons);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateCategory([FromBody] CategoryDto createCategory)
        {
            if (createCategory == null) return BadRequest(ModelState);


            var existingCategory = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim() == createCategory.Name.TrimEnd()).FirstOrDefault();

            if (existingCategory != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(createCategory);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while doin");
                return StatusCode(500, ModelState);
            }

            return Ok(categoryMap);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updateCategory) 
        {
            
            if (updateCategory == null)  return BadRequest(ModelState); 
            if(categoryId == 0)  return BadRequest(ModelState); 
            if(categoryId != updateCategory.Id) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (!_categoryRepository.CategoryExists(categoryId)) 
                return NotFound();

            var categoryMap = _mapper.Map<Category>(updateCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "somthing went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(categoryMap);
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var CategoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (CategoryToDelete == null) return NotFound();

            if (!_categoryRepository.DeleteCategory(CategoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }

    }
}
