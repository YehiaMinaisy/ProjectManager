using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using ProjectManager.Dtos;
using ProjectManager.Models;
using System.Security.Claims;
using System.Text;

namespace ProjectManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly Uri _baseUrl = new Uri("https://localhost:7154/api");
        private readonly HttpClient _httpClient;
        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseUrl;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register() {  return View(); }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto user)
        {
            if (ModelState.IsValid)
            {
                var requestUser = new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                };
                var data = JsonConvert.SerializeObject(requestUser);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Account/Register", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(user); 
        
        }
        public IActionResult Login() { return View(); }


        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto user)
        {
            LoginResponseDto? loginUser = null;


            if (ModelState.IsValid)
            {
                String data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Account/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    var returnedData = await response.Content.ReadAsStringAsync();
                    loginUser = JsonConvert.DeserializeObject<LoginResponseDto>(returnedData);
                }
            }
            if (loginUser != null)
            { 
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,loginUser.UserName),
                    new Claim(ClaimTypes.Role,loginUser.Role),
                    new Claim("token",loginUser.Token)
                };
                var claimIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimIdentity));
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
