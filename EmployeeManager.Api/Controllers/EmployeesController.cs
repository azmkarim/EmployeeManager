using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Api.Models;
using EmployeeManager.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManager.Api.Coontrollers
{
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository employeeRepository = null;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        
        [HttpGet]
        // [Authorize(Roles = "Manager")]
        [Authorize]
        public List<Employee> Get()
        {
            return employeeRepository.SelectAll();
        }

        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return employeeRepository.SelectByID(id);
        }

        [HttpPost]
        public void Post([FromBody]Employee emp)
        {
             if(ModelState.IsValid)
            {
                employeeRepository.Insert(emp);
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Employee emp)
        {
            if(ModelState.IsValid)
            {
                employeeRepository.Update(emp);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            employeeRepository.Delete(id);
        }
    }
}
