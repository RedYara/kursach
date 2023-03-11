using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Persistence.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupTypesController : Controller
    {
        private readonly _dbContext _context;

        public GroupTypesController(_dbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var groupTypes = await _context.GroupType.OrderBy(x => x.Id).ToListAsync();
            var groups = await _context.Group.OrderBy(x => x.Id).ToListAsync();
            var tuple = new Tuple<IEnumerable<GroupType>, IEnumerable<Group>>(groupTypes, groups);
            return View(tuple);
        }

        // GET: GroupTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GroupType == null)
            {
                return NotFound();
            }

            var groupType = await _context.GroupType
                .Include(g => g.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupType == null)
            {
                return NotFound();
            }

            return View(groupType);
        }

        // GET: GroupTypes/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Name");
            return View();
        }

        // POST: GroupTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroupType(GroupType groupType)
        {
            var toAdd = new GroupType
            {
                Name = groupType.Name,
                GroupId = groupType.GroupId
            };
            await _context.GroupType.AddAsync(toAdd);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        // GET: GroupTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GroupType == null)
            {
                return NotFound();
            }

            var groupType = await _context.GroupType.FindAsync(id);
            if (groupType == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Group, "Id", "Name", groupType.GroupId);
            return View(groupType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GroupType groupType)
        {
            try
            {
                var groupTypeToEdit = await _context.GroupType.FirstOrDefaultAsync(x => x.Id == groupType.Id);
                groupTypeToEdit.Name = groupType.Name;
                groupTypeToEdit.GroupId = groupType.GroupId;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupTypeExists(groupType.Id))
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
            var toDelete = await _context.GroupType.FirstOrDefaultAsync(x => x.Id == id);
            _context.GroupType.Remove(toDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<string> CreateGroupTypeAjax(int groupId, string name)
        {
            if (User.IsInRole("Admin"))
            {
                GroupType groupType = new() { Name = name, GroupId = groupId };
                await _context.GroupType.AddAsync(groupType);
                await _context.SaveChangesAsync();
                return name;
            }
            else return name;
        }
        private bool GroupTypeExists(int id)
        {
            return _context.GroupType.Any(e => e.Id == id);
        }
    }
}
