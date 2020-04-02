using EmployeeManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Repositories
{
    public interface ICountryRepository
    {
        List<Country> SelectAll();
    }
}
