using PockemonReviewApp.Data;
using PockemonReviewApp.Interfaces;
using PockemonReviewApp.Models;

namespace PockemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CountryExists(int id)
        {
            return _context.Countries.Any(v => v.Id == id);
        }


        public ICollection<Country> GetAllCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(x => x.Id == id).FirstOrDefault();
        }

        public Country GetCountryOfAnOwner(int ownerId)
        {
            return _context.Owners.Where(c => c.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _context.Owners.Where(d=> d.Country.Id == countryId).ToList();
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }
        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
        public bool Save()
        {
            var save = _context.SaveChanges();

            return save > 0 ? true : false;
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }
    }
}
