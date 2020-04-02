using EmployeeManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Repositories
{
    public class CountrySqlRepository : ICountryRepository
    {
        private readonly AppDbContext db = null;
        public CountrySqlRepository(AppDbContext dbContext)
        {
            this.db = dbContext;
        }
        public List<Country> SelectAll()
        {
            List<Country> country = db.Countries.FromSqlRaw("SELECT CountryId, Name FROM Countries").ToList();
            return country;
        }
    }
}
