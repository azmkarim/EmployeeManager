using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManager.ApiClient.Models;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using EmployeeManager.ApiClient.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManager.Mvc.Controllers
{
    public class SecurityController : Controller
    {
        private readonly HttpClient client = null;
        private string registerApiUrl = "";
        private string signinApiUrl = "";

        public SecurityController(HttpClient httpClient, IConfiguration configuration)
        {
            this.client = httpClient;
            registerApiUrl = configuration.GetValue<string>("AppSettings:RegisterApiUrl");
            signinApiUrl = configuration.GetValue<string>("AppSettings:signinApiUrl");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register obj)
        {
            if (ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(registerApiUrl, contentData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "User Registered Successfully !";
                }
                else
                {
                    ViewBag.Message = "Error While Calling Web API !";
                }
            }
            return View(obj);
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(obj);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(signinApiUrl, contentData);
                if (responseMessage.IsSuccessStatusCode)
                {
                    string stringReturnData = await responseMessage.Content.ReadAsStringAsync();
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    JwtInfo jwtInfo = JsonSerializer.Deserialize<JwtInfo>(stringReturnData, options);
                    // Check Token Format
                    if (!jwtHandler.CanReadToken(jwtInfo.token))
                    {
                        ModelState.AddModelError("", "The token doesn't seem to be in a proper JWT format.");
                    } else
                    {
                        var stream = jwtInfo.token;
                        var jsonToken = jwtHandler.ReadToken(stream);
                        var tokenS = jwtHandler.ReadToken(stream) as JwtSecurityToken;
                        HttpContext.Session.SetString("token", jwtInfo.token);
                        HttpContext.Session.SetString("username", obj.UserName);
                        var claims = tokenS.Claims;
                        var jwtPayload = "{";
                        foreach (Claim c in claims)
                        {
                            if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                            {
                                jwtInfo.UserName = c.Value;
                            }
                            else if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                            {
                                jwtInfo.UserRole = c.Value;
                            }
                            else if (c.Type == "FullName")
                            {
                                jwtInfo.FullName = c.Value;
                            }
                            jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
                        }
                        jwtPayload += "}";
                        HttpContext.Session.SetString("jwtPayload", jwtPayload);
                        HttpContext.Session.SetString("userFullName", jwtInfo.FullName);

                        //jwtInfo.UserName = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                        //jwtInfo.UserRole = tokenS.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                        //jwtInfo.FullName = tokenS.Claims.First(claim => claim.Type == "FullName").Value;
                        return RedirectToAction("List", "EmployeeManager");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                }
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("username");
            return RedirectToAction("SignIn", "Security");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
