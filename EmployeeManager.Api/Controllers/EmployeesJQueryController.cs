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
        public async Task<IActionResult> PostAsync([FromBody]Employee employee)
        {
            if (ModelState.IsValid)
            {
                await appDbContext.Employees.AddAsync(employee);
                await appDbContext.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = employee.EmployeeId }, employee);
            } else
            {
                return BadRequest();
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody]Employee employee)
        {
            if (ModelState.IsValid)
            {
                appDbContext.Employees.Update(employee);
                await appDbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            // Checkinf Whether Employee Exist !!!
            bool employeeExist = await appDbContext.Employees.AnyAsync(a => a.EmployeeId == id);
            if (employeeExist == false)
            {
                return NotFound();
            }
            Employee employee = await appDbContext.Employees.FindAsync(id);
            appDbContext.Remove(employee);
            await appDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
