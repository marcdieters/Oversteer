using Oversteer.Models;

namespace Oversteer.Webapp.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountries();
        Task UpsertCountry(Country country);
        Task RemoveCountry(Country country);
    }
}
