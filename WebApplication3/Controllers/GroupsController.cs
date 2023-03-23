using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsController : Controller
    {
        private readonly _dbContext _context;

        public GroupsController(_dbContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Group.OrderBy(x => x.Id).ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            _context.Group.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(Group group)
        {
            try
            {
                var groupToEdit = await _context.Group.FirstOrDefaultAsync(x => x.Id == group.Id);
                groupToEdit.Name = group.Name;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(@group.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var toDelete = await _context.Group.FirstOrDefaultAsync(x => x.Id == id);
            _context.Group.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        //[HttpPost]
        //public async Task<int> DeleteConfirmed(int id)
        //{
        //    var toDelete = await _context.Group.FirstOrDefaultAsync(x => x.Id == id);
        //    _context.Group.Remove(toDelete);
        //    await _context.SaveChangesAsync();
        //    return id;
        //}

        [HttpGet]
        public async Task<string> CreateGroupAjax(string name)
        {
            if (User.IsInRole("Admin"))
            {
                Group group = new() { Name = name };
                await _context.Group.AddAsync(group);
                await _context.SaveChangesAsync();
                return name;
            }
            else return name;
        }
        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }
    }
}
