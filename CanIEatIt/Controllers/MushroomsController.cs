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
    public class MushroomsController : Controller
    {
        private readonly CanIEatItContext _context;

        public MushroomsController(CanIEatItContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Mushrooms
        public async Task<IActionResult> Database(bool? edible, string mushroomLocation, string mushroomFamily, string searchString)
        {
            if (_context.Mushroom == null)
            {
                return Problem("Entity set 'MushroomContext.Mushroom' is null.");
            }

            IQueryable<string> locationQuery = from m in _context.Mushroom orderby m.Location select m.Location;

            IQueryable<string> familyQuery = from m in _context.Mushroom orderby m.Family select m.Family;

            var mushrooms = from m in _context.Mushroom select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                mushrooms = mushrooms.Where(x => x.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(mushroomLocation))
            {
                mushrooms = mushrooms.Where(x => x.Location == mushroomLocation);
            }

            if (!string.IsNullOrEmpty(mushroomFamily))
            {
                mushrooms = mushrooms.Where(x => x.Family == mushroomFamily);
            }

            if (edible.HasValue)
            {
                mushrooms = mushrooms.Where(x => x.Edible == edible);
            }

            var mushroomEdibleVM = new MushroomViewModel
            {
                Locations = new SelectList(await locationQuery.Distinct().ToListAsync()),
                Families = new SelectList(await familyQuery.Distinct().ToListAsync()),
                Mushrooms = await mushrooms.ToListAsync()
            };

            return View(mushroomEdibleVM);
        }

        // GET: Mushrooms/Details/5
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

        // GET: Mushrooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mushrooms/Create
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
                return RedirectToAction(nameof(Database));
            }
            return View(mushroom);
        }

        // GET: Mushrooms/Edit/5
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

        // POST: Mushrooms/Edit/5
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
                return RedirectToAction(nameof(Database));
            }
            return View(mushroom);
        }

        // GET: Mushrooms/Delete/5
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

        // POST: Mushrooms/Delete/5
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
            return RedirectToAction(nameof(Database));
        }

        private bool MushroomExists(int id)
        {
          return (_context.Mushroom?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
