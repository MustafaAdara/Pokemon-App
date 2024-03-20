using PockemonReviewApp.Models;

namespace PockemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetAllCountries();

        Country GetCountry(int id);

        Country GetCountryOfAnOwner(int ownerId);

        ICollection<Owner> GetOwnersFromCountry(int countryId); 
        bool CountryExists(int id);

        bool CreateCountry(Country country);

        bool UpdateCountry(Country country);

        bool DeleteCountry(Country country);
        bool Save();
    }
}
