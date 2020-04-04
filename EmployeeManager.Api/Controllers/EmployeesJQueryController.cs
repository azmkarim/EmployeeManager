using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace EmployeeManager.Api.Controllers
{
    [Route("api/[controller]")]
    public class EmployeesJQueryController : Controller
    {
        private readonly AppDbContext appDbContext = null;
        public EmployeesJQueryController(AppDbContext dbContext)
        {
            this.appDbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<Employee> employeesList = await appDbContext.Employees.ToListAsync();
            Console.WriteLine("Listing Employee Done. Returning ...");
            return Ok(employeesList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            // Checkinf Whether Employee Exist !!!
            bool employeeExist = await appDbContext.Employees.AnyAsync(a => a.EmployeeId == id);
            if (employeeExist == false)
            {
                return NotFound();
            }
            Employee empData = await appDbContext.Employees.FindAsync(id);
            return Ok(empData);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
