using AutoMapper;
using PockemonReviewApp.Dto;
using PockemonReviewApp.Models;

namespace PockemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();


            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<CountryDto, Country>();
            CreateMap<Country, CountryDto>();

            CreateMap<OwnerDto, Owner>();
            CreateMap<Owner, OwnerDto>();


            CreateMap<ReviewDto, Review>();
            CreateMap<Review, ReviewDto>();

            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<Reviewer, ReviewerDto>();

        }
    }
}
