using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProjectManager.Data;
using ProjectManager.Models;
//using System.Data.Entity.Infrastructure;
using System.Text;

namespace ProjectManager.Controllers
{
    public class ProjectTasksController : Controller
    {
        private readonly ProjectsDbContext _context;
        private readonly Uri _baseUrl = new Uri("https://localhost:7154/api");
        private readonly HttpClient _httpClient;
        public string StatusMessage { get; set; }

        public ProjectTasksController(ProjectsDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseUrl;
        }
        public async Task<IActionResult> PartialFilteredTasks(DateTime? startDate, DateTime? endDate)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Task/GetAll");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ProjectTask>>(data);
            }
            if (startDate != null)
            {
                tasks = tasks.Where(t => t.DueDate >= startDate).ToList();
            }
            if (endDate != null)
            {
                tasks = tasks.Where(t => t.DueDate <= endDate).ToList();
            }
            return PartialView( tasks);
        }


        // GET: ProjectTasks
        public async Task<IActionResult> Index()
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Task/GetAll");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                tasks = JsonConvert.DeserializeObject<List<ProjectTask>>(data);
            }
            
            
            return View("Index",tasks);
        }

        // GET: ProjectTasks/Details/5
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
        public async Task<IActionResult> PartialSubTaskDetails(int? id)
        {
            /*if (id == null)
            {
                return NotFound();
            }*/
            ProjectTask? task = new ProjectTask();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/task/GetById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                task = JsonConvert.DeserializeObject<ProjectTask>(data);
                return PartialView("_SubTaskDetails", task);
            }

            /* if (projectTask == null)
             {
                 return NotFound();
             }*/

            /*return PartialView(projectTask);*/
            return NotFound();
        }

        // GET: ProjectTasks/Create
        public IActionResult Create()
        {

            ViewData["ParentId"] = new SelectList(_context.Tasks, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DueDate,ProjectId,ParentId")] ProjectTask projectTask, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        var mimeType = file.ContentType;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            await file.CopyToAsync(stream);
                        }
                        projectTask.AttachmentName = file.FileName;
                        projectTask.AttachmentType = file.ContentType;
                        projectTask.AttachmentData = ConvertToByteArray(filePath);

                    }
                    string data = JsonConvert.SerializeObject(projectTask);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage resposne = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/task/Create/{projectTask.ProjectId}", content);
                    if (resposne.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }


                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return View(projectTask);
        }

        private byte[] ConvertToByteArray(string filePath)
        {
            Byte[] fileData;
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fileData = reader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }

        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ProjectTask? task = new ProjectTask();
            HttpResponseMessage resposne = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Task/GetById/{id}");
            if (resposne.IsSuccessStatusCode)
            {
                var data = await resposne.Content.ReadAsStringAsync();
                task = JsonConvert.DeserializeObject<ProjectTask>(data);
                ViewData["ParentNames"] = new SelectList(_context.Tasks.Where(t => t.Id != id), "Id", "Name", task.ParentId);
                ViewData["ProjectNames"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", task.ProjectId);
                return View(task);
            }
            return NotFound();
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DueDate,ProjectId,ParentId")] ProjectTask projectTask)
        {
            if (id != projectTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (projectTask.ParentId == 0)
                    {
                        projectTask.ParentId = null;
                    }
                    var data = JsonConvert.SerializeObject(projectTask);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/Task/Update/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    if (!ProjectTaskExists(projectTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            ViewData["ParentId"] = new SelectList(_context.Tasks, "Id", "Name", projectTask.ParentId);
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", projectTask.ProjectId);
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["ErrorMessage"] = null;
            if (id == null)
            {
                return NotFound();
            }

            ProjectTask? task = new ProjectTask();
            HttpResponseMessage resposne = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Task/GetById/{id}");
            if (resposne.IsSuccessStatusCode)
            {
                var data = await resposne.Content.ReadAsStringAsync();
                task = JsonConvert.DeserializeObject<ProjectTask>(data);
                ViewData["ParentNames"] = new SelectList(_context.Tasks.Where(t => t.Id != id), "Id", "Name", task.ParentId);
                ViewData["ProjectNames"] = new SelectList(_context.Projects, "ProjectId", "ProjectName", task.ProjectId);
                return View(task);
            }
            return NotFound();
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {


            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/task/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                var message = "This task is a Parent of other sub tasks they must be deleted first";


                ViewData["ErrorMessage"] = message;
            }
            return BadRequest();





        }

        private bool ProjectTaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
