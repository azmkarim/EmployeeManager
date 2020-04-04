using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManager.JQuery.Controllers
{
    public class EmployeeManagerController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Insert()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            ViewBag.EmployeeID = id;
            return View();
        }

        public IActionResult Delete(int id)
        {
            ViewBag.EmployeeID = id;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
    }
}
