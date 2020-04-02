using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeManager.ApiClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManager.ApiClient.Controllers
{
    public class EmployeeManagerController : Controller
    {
        private readonly HttpClient client = null;
        private string baseUrl = "";
        private string employeesApiUrl = "";
        private string countriesApiUrl = "";

        public EmployeeManagerController(HttpClient httpClient, IConfiguration configuration)
        {
            this.client = httpClient;
            baseUrl = configuration.GetValue<string>("AppSettings:BaseUrl");
            employeesApiUrl = configuration.GetValue<string>("AppSettings:EmployeesApiUrl");
            countriesApiUrl = configuration.GetValue<string>("AppSettings:CountriesApiUrl");
        }

        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> ListAsync()
        {
            // Adding Jwt Token To Header
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage responseMessage = await client.GetAsync(employeesApiUrl);
            string stringData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Employee> empData = JsonSerializer.Deserialize<List<Employee>>(stringData, options);
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                TempData["Message"] = "You Are Not Unauthorized To View This Page !";
                return View();
            }
            else
            {
                return View(empData);
            }
            // TempData["Message"] = HttpContext.Session.GetString("jwtPayload");
            // TempData["Message"] = HttpContext.Session.GetString("token");
        }

        public async Task<IActionResult> InsertAsync()
        {
            await FillCountriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertAsync(Employee employee)
        {
            await FillCountriesAsync();
            if(ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(employee);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(employeesApiUrl, contentData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Employee Inserted Successfully !";
                }
                else
                {
                    ViewBag.Message = "Error While Calling Web API !";
                }
            }
            return View(employee);
        }

        public async Task<IActionResult> UpdateAsync(int id)
        {
            await FillCountriesAsync();
            HttpResponseMessage responseMessage = await client.GetAsync($"{employeesApiUrl}/{id}");
            string stringData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Employee empData = JsonSerializer.Deserialize<Employee>(stringData, options);
            return View(empData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(Employee employee)
        {
            await FillCountriesAsync();
            if(ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(employee);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync($"{employeesApiUrl}/{employee.EmployeeId}", contentData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Employee Inserted Successfully !";
                }
                else
                {
                    ViewBag.Message = "Error While Calling Web API !";
                }
            }
            return View(employee);
        }

        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDeleteAsync(int id)
        {
            HttpResponseMessage responseMessage = await client.GetAsync($"{employeesApiUrl}/{id}");
            string stringData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Employee empData = JsonSerializer.Deserialize<Employee>(stringData, options);
            return View(empData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(int employeeId)
        {
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{employeesApiUrl}/{employeeId}");
            if (responseMessage.IsSuccessStatusCode)
            {
                ViewBag.Message = "Employee Inserted Successfully !";
            }
            else
            {
                ViewBag.Message = "Error While Calling Web API !";
            }
            return RedirectToAction("List");
        }

        public async Task<bool> FillCountriesAsync()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(countriesApiUrl);
            string stringData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Country> listCountries = JsonSerializer.Deserialize<List<Country>>(stringData, options);
            List<SelectListItem> countries = (from c in listCountries
                                              select new SelectListItem()
                                              {
                                                  Text = c.Name,
                                                  Value = c.Name
                                              }).ToList();
            ViewBag.Countries = countries;
            return true;
        }
    }
}
