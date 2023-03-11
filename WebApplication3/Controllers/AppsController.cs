using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using WebApplication3.Models;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Controllers
{
    [Authorize]
    public class AppsController : Controller
    {
        private readonly _dbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AppsController(_dbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var app = await _context.Apps.ToListAsync();
            var groups = await _context.Group.ToListAsync();
            var tuple = new Tuple<IEnumerable<App>, IEnumerable<Group>>(app,groups);
            return View(tuple);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SortedApps(int groupId)
        {
            IEnumerable<App> apps;

            if (groupId != 0)
            {
                apps = await _context.Apps.Where(x => x.GroupId == groupId).ToListAsync();
            }
            else
            {
                apps = await _context.Apps.ToListAsync();
            }
           
            return PartialView(apps);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Apps == null)
            {
                return NotFound();
            }

            var app = await _context.Apps
                .FirstOrDefaultAsync(m => m.Id == id);
            app.Screenshots = _context.Screenshots.Where(x => x.AppId == id).ToList();
            app.DownloadLink = "https://" + Request.Host.Value + "/" + app.DownloadLink;
            if (app == null)
            {
                return NotFound();
            }

            return View(app);
        }

        public IActionResult Create()
        {
            if (User.IsInRole("Admin"))
                return View();
            else return Redirect("Index");
        }

        [HttpPost, DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue, ValueLengthLimit = Int32.MaxValue)]
        public async Task<IActionResult> CreateApp(CreateAppRequest app)
        {
            string uploadFolder = Path.Combine(_environment.WebRootPath, "appsFolder");
            string path = new(_environment.WebRootPath + "/appsFolder/" + $"{app.Apk.FileName.Remove(app.Apk.FileName.Length - 4)}_folder");
            try
            {
                if (Directory.Exists(uploadFolder))
                {
                    string appFolder = Path.Combine(path, "app");
                    try
                    {
                        Directory.CreateDirectory(appFolder);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ошибка при добавления дополнительных папок :", e.ToString());
                    }
                }
                else
                {
                    Directory.CreateDirectory(path);
                    string appFolder = Path.Combine(path, "app");
                    try
                    {
                        Directory.CreateDirectory(appFolder);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ошибка при добавления дополнительных папок :", e.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            string folderUpload = new(_environment.WebRootPath + $"/appsFolder/{app.Apk.FileName.Remove(app.Apk.FileName.Length - 4)}_folder/app/");
            string filePath = new(folderUpload + app.Apk.FileName);
            string appPath = new($"/appsFolder/{app.Apk.FileName.Remove(app.Apk.FileName.Length - 4)}_folder/app/" + app.Apk.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await app.Apk.CopyToAsync(fileStream);

            #region Конвертирование лого
            using MemoryStream ms = new();

            var formFiles = app.Logo;
            formFiles.CopyTo(ms);
            var bytes = ms.ToArray();
            var iconBase64 = Convert.ToBase64String(bytes);

            #endregion
            #region Конвертирование всех скринов
            var screensToAdd = new List<Screenshots>();
            var jpgEncoder = new JpegEncoder
            {
                Quality = 90
            };
            foreach (var item in app.Screenshots)
            {
                using MemoryStream mss = new();
                Image image = Image.Load(item.OpenReadStream());
                image.Mutate(x => x.Resize(800, 600));
                image.Save(mss, jpgEncoder);
                var screenBytes = mss.ToArray();
                var screenBase64 = Convert.ToBase64String(screenBytes);
                screensToAdd.Add(new Screenshots { Base64 = screenBase64 });
            }
            #endregion

            var group = await _context.Group.Where(x => x.Id == app.GroupId).FirstOrDefaultAsync();
            var groupType = await _context.GroupType.Where(x => x.Id == app.GroupTypeId).FirstOrDefaultAsync();

            _context.Apps.Add(new App
            {
                CreatedDate = DateTime.Now,
                Description = app.Description,
                DownloadLink = appPath,
                Logo = iconBase64,
                Group = group,
                GroupType = groupType,
                Name = app.Name,
                Screenshots = screensToAdd,
                Version = app.Version
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (User.IsInRole("Admin"))
            {
                if (id == null || _context.Apps == null)
                {
                    return NotFound();
                }

                var app = await _context.Apps.FindAsync(id);
                var appToEdit = new EditAppRequest
                {
                    Version = app.Version,
                    CreatedDate = app.CreatedDate,
                    Description = app.Description,
                    LogoUrl = app.Logo,
                    GroupId = app.GroupId,
                    GroupTypeId = app.GroupTypeId,
                    Name = app.Name,
                    ScreenshotsList = await _context.Screenshots.Where(x => x.AppId == id).ToListAsync()
                };
                if (appToEdit == null)
                {
                    return NotFound();
                }
                return View(appToEdit);
            }
            else return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAppRequest app)
        {

            if (User.IsInRole("Admin"))
            {
                if (id != app.Id)
                {
                    return NotFound();
                }


                try
                {
                    var appToUpdate = _context.Apps.Where(x => x.Id == app.Id).FirstOrDefault();
                    if (app.Apk != null)
                    {
                        string folderUpload = new(_environment.WebRootPath + $"/appsFolder/{app.Apk.FileName.Remove(app.Apk.FileName.Length - 4)}_folder/app/");
                        string filePath = new(folderUpload + app.Apk.FileName);
                        string appPath = new($"/appsFolder/{app.Apk.FileName.Remove(app.Apk.FileName.Length - 4)}_folder/app/" + app.Apk.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await app.Apk.CopyToAsync(fileStream);
                        appToUpdate.DownloadLink = appPath;
                    }

                    #region Конвертирование лого
                    using MemoryStream ms = new();

                    if (app.Logo != null)
                    {
                        var formFiles = app.Logo;
                        formFiles.CopyTo(ms);
                        var bytes = ms.ToArray();
                        var iconBase64 = Convert.ToBase64String(bytes);
                        appToUpdate.Logo = iconBase64;
                    }

                    #endregion
                    #region Конвертирование всех скринов
                    if (app.Screenshots != null)
                    {
                        var screensToAdd = new List<Screenshots>();
                        var jpgEncoder = new JpegEncoder
                        {
                            Quality = 90
                        };
                        foreach (var item in app.Screenshots)
                        {
                            using MemoryStream mss = new();
                            Image image = Image.Load(item.OpenReadStream());
                            image.Mutate(x => x.Resize(800, 600));
                            image.Save(mss, jpgEncoder);
                            var screenBytes = mss.ToArray();
                            var screenBase64 = Convert.ToBase64String(screenBytes);
                            screensToAdd.Add(new Screenshots { Base64 = screenBase64 });
                        }
                        appToUpdate.Screenshots = screensToAdd;
                    }
                    #endregion

                    if (app.CreatedDate != null) appToUpdate.CreatedDate = app.CreatedDate;
                    if (app.Description != null) appToUpdate.Description = app.Description;
                    if (app.Name != null) appToUpdate.Name = app.Name;
                    if (app.Version != null) appToUpdate.Version = app.Version;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppExists(app.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return Redirect("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null || _context.Apps == null)
                {
                    return NotFound();
                }

                var app = await _context.Apps
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (app == null)
                {
                    return NotFound();
                }

                return View(app);
            }
            else return Redirect("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (User.IsInRole("Admin"))
            {
                if (_context.Apps == null)
                {
                    return Problem("Entity set '_dbContext.Apps'  is null.");
                }
                var app = await _context.Apps.FindAsync(id);
                if (app != null)
                {
                    _context.Apps.Remove(app);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return Redirect("index");
        }

        [HttpGet]
        public async Task<List<Group>> GetGroups()
        {
            List<Group> groups = _context.Group.ToList();
            return groups;
        } 
        
        [HttpGet]
        public async Task<List<GroupType>> GetGroupTypes(int id)
        {
            List<GroupType> groupTypes = _context.GroupType.Where(x => x.GroupId == id).ToList();
            return groupTypes;
        }

        private bool AppExists(int id)
        {
            return _context.Apps.Any(e => e.Id == id);
        }
    }
}
