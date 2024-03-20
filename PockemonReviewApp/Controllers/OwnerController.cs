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
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllOwners()
        {
            var owners = _ownerRepository.GetOwners();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }


        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public ActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExits(ownerId))
                return NotFound();

            var owner = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);

        }


        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public ActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExits(ownerId))
                return NotFound();

            var owner = _ownerRepository.GetPokemonByOwner(ownerId);

            if (!ModelState.IsValid) 
                    return BadRequest(ModelState);
            return Ok(owner);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public ActionResult CreateOwner([FromQuery] int countryId ,[FromBody] OwnerDto createOwner)
        {
            /* Egypt 
            var owner = new Owner { FirstName = createOwner.FirstName,
                                    LastName = createOwner.LastName ,
                                    Gym = createOwner.Gym};

            _ownerRepository.CreateOwner(owner);
            owner.Country = _countryRepository.GetCountry(countryId);
           if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(owner);
            */
            if (createOwner == null) return BadRequest(ModelState);


            var existingOwner = _ownerRepository.GetOwners()
                .Where(c => c.LastName.Trim() == createOwner.LastName.TrimEnd()).FirstOrDefault();

            if (existingOwner != null)
            {
                ModelState.AddModelError("", "Owner Already Exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(createOwner);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while doin");
                return StatusCode(500, ModelState);
            }

            return Ok(ownerMap);
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updateOwner)
        {

            if (updateOwner == null) return BadRequest(ModelState);
            if (ownerId == 0) return BadRequest(ModelState);
            if (ownerId != updateOwner.Id) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (!_ownerRepository.OwnerExits(ownerId)) return NotFound();

            var ownerMap = _mapper.Map<Owner>(updateOwner);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "somthing went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(ownerMap);
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.OwnerExits(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }
    }
}


