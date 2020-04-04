using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            ViewBag.EmployeeId = id;
            return View();
        }

        public IActionResult Delete(int id)
        {
            ViewBag.EmployeeId = id;
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
