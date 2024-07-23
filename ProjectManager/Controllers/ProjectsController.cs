using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectManager.Data;
using ProjectManager.Models;
using System.Text;

namespace ProjectManager.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsDbContext _context;
        private readonly Uri _baseUrl = new Uri("https://localhost:7154/api");
        private readonly HttpClient _httpClient;

        public ProjectsController(ProjectsDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseUrl;
        }

        public async Task<IActionResult> Index()
        {
            List<Project> projects = new List<Project>();
            HttpResponseMessage response = _httpClient.GetAsync(_baseUrl + "/Project/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                projects = JsonConvert.DeserializeObject<List<Project>>(data);
            }

            return View(projects);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string data = JsonConvert.SerializeObject(project);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Project/Create", content);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return BadRequest("can't create this project");

                }

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Project? project = new Project();
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Project/Get/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = await responseMessage.Content.ReadAsStringAsync();
                project = JsonConvert.DeserializeObject<Project>(data);
                return View(project);
            }
            return BadRequest("Project not found");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            string data = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await _httpClient.PutAsync(_httpClient.BaseAddress + "/Project/Update/" + project.ProjectId, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }




            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Project? project = new Project();
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Project/Get/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                project = JsonConvert.DeserializeObject<Project>(data);
                return View(project);
            }
            return BadRequest(responseMessage.Headers);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteProject(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Project? project = new Project();
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Project/Get/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                project = JsonConvert.DeserializeObject<Project>(data);
                HttpResponseMessage deleteMessage = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Project/Delete/{id}");
                if (deleteMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }
            return BadRequest(responseMessage.Headers);

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Project project = new Project();
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "/Project/Get/" + id);
            if (response.IsSuccessStatusCode)
            {

                var data = await response.Content.ReadAsStringAsync();
                project = JsonConvert.DeserializeObject<Project>(data);
            }
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
        [ActionName("CreateTask")]
        public async Task<IActionResult> CreateTask(int? projectId)
        {
            if (projectId == null || projectId == 0) { return NotFound(); }
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.ProjectId = projectId;
            return View(project);

        }
        [HttpPost, ActionName("CreateTask")]
        public async Task<IActionResult> CreateTask(ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(task);

        }
        public IActionResult MyPage()
        {
            return View();
        }

    }
}
