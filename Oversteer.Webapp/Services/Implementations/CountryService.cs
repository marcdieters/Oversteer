using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oversteer.Models;
using Oversteer.Webapp.Data;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Oversteer.Webapp.Services
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _db;

        public CountryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<List<Country>> GetAllCountries()
        {
            var countries = _db.Countries.OrderBy(c => c.Name).ToList();
            return Task.FromResult(countries);
        }

        public Task RemoveCountry(Country Country)
        {
            if (_db.Countries.Any(c => c.Id == Country.Id))
            {
                var country = _db.Countries.First(c => c.Id == Country.Id);
                _db.Countries.Remove(country);
                _db.SaveChanges();
            }

            return Task.CompletedTask;
        }

        public Task UpsertCountry(Country country)
        {
            if (!_db.Countries.Any(c => c.Id == country.Id))
            {
                _db.Countries.Add(country);
            }
            else
            {
                _db.Entry(country).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return Task.CompletedTask;
        }
    }
}