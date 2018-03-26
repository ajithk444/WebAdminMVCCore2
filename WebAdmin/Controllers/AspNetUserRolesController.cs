using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    [RequireHttps]
    [Authorize(Roles ="Admin")]

    public class AspNetUserRolesController : Controller
    {
        private readonly IdentityDbContext _context;

        public AspNetUserRolesController(IdentityDbContext context)
        {
            _context = context;
        }

        // GET: AspNetUserRoles
        public async Task<IActionResult> Index()
        {
            var identityDbContext = _context.AspNetUserRoles.Include(a => a.Role).Include(a => a.User);
            return View(await identityDbContext.ToListAsync());
        }

        // GET: AspNetUserRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Create
        public IActionResult Create(string userId)
        {
            AspNetUserRole aspNetUserRole = new AspNetUserRole();
            aspNetUserRole.UserId = userId;
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles.OrderBy(r => r.Name), "Id", "Name");
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUserRole);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "aspNetUsers",new { id=aspNetUserRole.UserId});
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles.OrderBy(r => r.Name), "Id", "Name");
            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles.SingleOrDefaultAsync(m => m.UserId == id);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            if (id != aspNetUserRole.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUserRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserRoleExists(aspNetUserRole.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Id", aspNetUserRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Delete/5
        public async Task<IActionResult> Delete(string userId,string roleId)
        {
            if (userId == null || roleId== null )
            {
                return NotFound();
            }

            var aspNetUserRole = await _context.AspNetUserRoles
                .Include(a => a.Role)
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.UserId == userId && m.RoleId==roleId);
            if (aspNetUserRole == null)
            {
                return NotFound();
            }

            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId, string roleId)
        {
            var aspNetUserRole = await _context.AspNetUserRoles.SingleOrDefaultAsync(m => m.UserId == userId && m.RoleId==roleId);
            _context.AspNetUserRoles.Remove(aspNetUserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","AspNetUsers",new { id= userId});
        }

        private bool AspNetUserRoleExists(string id)
        {
            return _context.AspNetUserRoles.Any(e => e.UserId == id);
        }
    }
}
