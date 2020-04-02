using EmployeeManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Repositories
{
    public class EmployeeSqlRepository : IEmployeeRepository
    {
        private readonly AppDbContext db = null;
        public EmployeeSqlRepository(AppDbContext dbContext)
        {
            this.db = dbContext;
        }
        public List<Employee> SelectAll()
        {
            List<Employee> empData = db.Employees.FromSqlRaw("SELECT EmployeeID, FirstName, LastName, Title, BirthDate, HireDate, Country, Notes FROM Employees ORDER BY EmployeeID ASC").ToList();
            return empData;
        }

        public Employee SelectByID(int id)
        {
            Employee empData = db.Employees.FromSqlRaw("SELECT EmployeeID, FirstName, LastName, Title, BirthDate, HireDate, Country, Notes FROM Employees WHERE EmployeeID = {0}", id).SingleOrDefault();
            return empData;
        }

        public void Insert(Employee emp)
        {
            int count = db.Database.ExecuteSqlRaw("INSERT INTO Employees(FirstName, LastName, Title, BirthDate, HireDate, Country, Notes)  VALUES({0},{1},{2},{3},{4},{5},{6})", emp.FirstName, emp.LastName, emp.Title, emp.BirthDate, emp.HireDate, emp.Country, emp.Notes);
        }

        public void Update(Employee employee)
        {
            int count = db.Database.ExecuteSqlRaw("UPDATE Employees SET FirstName={0}, LastName={1}, Title={2}, BirthDate={3}, HireDate={4}, Country={5}, Notes={6} WHERE EmployeeID={7}", employee.FirstName, employee.LastName, employee.Title, employee.BirthDate, employee.HireDate, employee.Country, employee.Notes, employee.EmployeeId);
        }
        public void Delete(int id)
        {
            int count = db.Database.ExecuteSqlRaw("DELETE FROM Employees WHERE EmployeeID={0}", id);
        }
    }
}
