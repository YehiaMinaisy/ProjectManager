using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectManager.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ProjectManager.Controllers
{
    [Authorize(Roles ="User")]
    public class UserTasksController : Controller
    {
        private readonly Uri _baseUrl = new Uri("https://localhost:7154/api");
        private readonly HttpClient _httpClient;
        public UserTasksController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress =_baseUrl;
            
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProjectTask? task = new ProjectTask();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/task/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                task = JsonConvert.DeserializeObject<ProjectTask>(data);
                return View(task);
            }
            return NotFound("Task Not Found");


        }
        public async Task<IActionResult> PartialFilteredTasks(DateTime? startDate, DateTime? endDate)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.User.FindFirstValue("token"));
            var data = JsonConvert.SerializeObject(HttpContext.User.FindFirstValue(ClaimTypes.Name));
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/UserTasks/GetAllUserTasks", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ProjectTask>>(responseData);

            }
            if (startDate != null)
            {
                tasks = tasks.Where(t => t.DueDate >= startDate).ToList();
            }
            if (endDate != null)
            {
                tasks = tasks.Where(t => t.DueDate <= endDate).ToList();
            }
            return PartialView(tasks);
        }
        public async Task<IActionResult> Index()
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            
           _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",HttpContext.User.FindFirstValue("token"));
            var data = JsonConvert.SerializeObject(HttpContext.User.FindFirstValue(ClaimTypes.Name));
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/UserTasks/GetAllUserTasks", content);
            if (response.IsSuccessStatusCode) 
            { 
                var responseData = await response.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ProjectTask>>(responseData);

            }

            return View(tasks);
        }
        public async Task<IActionResult> AddTask()
        {
            var userTasks = new List<ProjectTask>();
            var allTasks = new List<ProjectTask>();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.User.FindFirstValue("token"));
            var data = JsonConvert.SerializeObject(HttpContext.User.FindFirstValue(ClaimTypes.Name));
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage userResponse = await _httpClient.PostAsync(_httpClient.BaseAddress + "/UserTasks/GetAllUserTasks", content);
            if (userResponse.IsSuccessStatusCode)
            {
                var responseData = await userResponse.Content.ReadAsStringAsync();
                userTasks = JsonConvert.DeserializeObject<List<ProjectTask>>(responseData);

            }
            HttpResponseMessage tasksResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Task/GetAll");
            if (tasksResponse.IsSuccessStatusCode)
            {
                var responseData = await tasksResponse.Content.ReadAsStringAsync();
                allTasks = JsonConvert.DeserializeObject<List<ProjectTask>>(responseData);
            }
            return PartialView("_AddTask",allTasks.Except(userTasks));

        }
        [HttpPost]
        public async Task<IActionResult> AddTask(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.User.FindFirstValue("token"));
            var data = JsonConvert.SerializeObject(HttpContext.User.FindFirstValue(ClaimTypes.Name));
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/UserTasks/AddUserTask/{id}", content);
            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
            }
            return BadRequest("couldn't add this task");

        }
        public async Task<IActionResult> LeaveTask(int? id)
        {
            if (id == null) 
            {
                return BadRequest();
            }
            var task = new ProjectTask();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/task/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                task = JsonConvert.DeserializeObject<ProjectTask>(data);
                return View(task);
            }

            return NotFound("Task not found");
        }
        [HttpPost("LeaveTaskPost")]
        public async Task<IActionResult> LeaveTaskPost(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.User.FindFirstValue("token"));
            
            var data = JsonConvert.SerializeObject(HttpContext.User.FindFirstValue(ClaimTypes.Name));
            
            StringContent content = new StringContent(data,Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/UserTasks/DeleteUserTask/{id}", content);
            
            if (response.IsSuccessStatusCode) 
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
