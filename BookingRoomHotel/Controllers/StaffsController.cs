using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingRoomHotel.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookingRoomHotel.Controllers
{
    public class StaffsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Staffs
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
              return _context.Staffs != null ?
                          PartialView(await _context.Staffs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Staffs'  is null.");
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return PartialView(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] [Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return PartialView(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm] [Bind("Id,Name,Email,Phone,DateOfBirth,Address,Pw,Role")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            return PartialView(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Staffs == null)
            {
                return NotFound();
            }

            var staff = await _context.Staffs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return PartialView(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromForm] string id)
        {
            if (_context.Staffs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Staffs'  is null.");
            }
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(string id)
        {
          return (_context.Staffs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
