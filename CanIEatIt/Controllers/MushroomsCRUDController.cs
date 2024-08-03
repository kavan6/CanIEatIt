using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CanIEatIt.Data;
using CanIEatIt.Models;

namespace CanIEatIt.Controllers
{
    public class MushroomsCRUDController : Controller
    {
        private readonly CanIEatItContext _context;

        public MushroomsCRUDController(CanIEatItContext context)
        {
            _context = context;
        }

        // GET: MushroomsCRUD
        public async Task<IActionResult> Index()
        {
              return _context.Mushroom != null ? 
                          View(await _context.Mushroom.ToListAsync()) :
                          Problem("Entity set 'CanIEatItContext.Mushroom'  is null.");
        }

        // GET: MushroomsCRUD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mushroom == null)
            {
                return NotFound();
            }

            var mushroom = await _context.Mushroom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mushroom == null)
            {
                return NotFound();
            }

            return View(mushroom);
        }

        // GET: MushroomsCRUD/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MushroomsCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Family,Location,CapDiameter,Height,Edible,EdibleDescription,CapDescription,StemDescription,GillDescription,SporeDescription,MicroscopicDescription,Note")] Mushroom mushroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mushroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mushroom);
        }

        // GET: MushroomsCRUD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mushroom == null)
            {
                return NotFound();
            }

            var mushroom = await _context.Mushroom.FindAsync(id);
            if (mushroom == null)
            {
                return NotFound();
            }
            return View(mushroom);
        }

        // POST: MushroomsCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Family,Location,CapDiameter,Height,Edible,EdibleDescription,CapDescription,StemDescription,GillDescription,SporeDescription,MicroscopicDescription,Note")] Mushroom mushroom)
        {
            if (id != mushroom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mushroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MushroomExists(mushroom.Id))
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
            return View(mushroom);
        }

        // GET: MushroomsCRUD/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mushroom == null)
            {
                return NotFound();
            }

            var mushroom = await _context.Mushroom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mushroom == null)
            {
                return NotFound();
            }

            return View(mushroom);
        }

        // POST: MushroomsCRUD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mushroom == null)
            {
                return Problem("Entity set 'CanIEatItContext.Mushroom'  is null.");
            }
            var mushroom = await _context.Mushroom.FindAsync(id);
            if (mushroom != null)
            {
                _context.Mushroom.Remove(mushroom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MushroomExists(int id)
        {
          return (_context.Mushroom?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
