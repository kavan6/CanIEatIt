using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CanIEatIt.Data;
using CanIEatIt.Models;
using CanIEatIt.Services;

namespace CanIEatIt.Controllers
{

    public class MushroomsController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        private readonly CanIEatItContext _context;

        public MushroomsController(CanIEatItContext context, IServiceRepository services)
        {
            _context = context;
            _serviceRepository = services;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Mushrooms
        public async Task<IActionResult> Database(
                                                  string searchName, string searchFamily, string searchLocation, 
                                                  string searchCapDiameter, string searchStemHeight, bool? searchEdible, 
                                                  string searchEdibleDes, string searchCapDes, string searchStemDes, 
                                                  string searchGillDes, string searchSporeDes, string searchMicroDes, 
                                                  string searchNote
                                                 )
        {
            if (_context.Mushroom == null)
            {
                return Problem("Entity set 'MushroomContext.Mushroom' is null.");
            }

            IQueryable<string> familyQuery = from m in _context.Mushroom orderby m.Family select m.Family;


            var mushrooms = from m in _context.Mushroom select m;

            #region LINQ Searches

            if (!string.IsNullOrEmpty(searchName))
            {
                mushrooms = mushrooms.Where(x => x.Name!.ToUpper().Contains(searchName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchFamily))
            {
                mushrooms = mushrooms.Where(x => x.Family == searchFamily);
            }

            if (!string.IsNullOrEmpty(searchLocation))
            {
                mushrooms = mushrooms.Where(x => x.Location!.ToUpper().Contains(searchLocation.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchCapDiameter))
            {
                mushrooms = mushrooms.Where(x => x.CapDiameter!.ToUpper().Contains(searchCapDiameter.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchStemHeight))
            {
                mushrooms = mushrooms.Where(x => x.StemHeight!.ToUpper().Contains(searchStemHeight.ToUpper()));
            }

            if (searchEdible.HasValue)
            {
                mushrooms = mushrooms.Where(x => x.Edible == searchEdible);
            }

            if (!string.IsNullOrEmpty(searchCapDes))
            {
                mushrooms = mushrooms.Where(x => x.CapDescription!.ToUpper().Contains(searchCapDes.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchStemDes))
            {
                mushrooms = mushrooms.Where(x => x.StemDescription!.ToUpper().Contains(searchStemDes.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchGillDes))
            {
                mushrooms = mushrooms.Where(x => x.GillDescription!.ToUpper().Contains(searchGillDes.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchSporeDes))
            {
                mushrooms = mushrooms.Where(x => x.SporeDescription!.ToUpper().Contains(searchSporeDes.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchMicroDes))
            {
                mushrooms = mushrooms.Where(x => x.MicroscopicDescription!.ToUpper().Contains(searchMicroDes.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchNote))
            {
                mushrooms = mushrooms.Where(x => x.Note!.ToUpper().Contains(searchNote.ToUpper()));
            }
            #endregion

            var mushroomEdibleVM = new MushroomViewModel
            { 
                Locations = new SelectList(await _serviceRepository.populateLocations(), "Value", "Text"),
                CapDiameters = new SelectList(await _serviceRepository.populateCapDiameters(), "Value", "Text"),
                StemHeights = new SelectList(await _serviceRepository.populateStemHeights(), "Value", "Text"),
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
        public async Task<IActionResult> Create([Bind("Id,Name,Family,Location,CapDiameter,StemHeight,Edible,EdibleDescription,CapDescription,StemDescription,GillDescription,SporeDescription,MicroscopicDescription,Note")] Mushroom mushroom)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Family,Location,CapDiameter,StemHeight,Edible,EdibleDescription,CapDescription,StemDescription,GillDescription,SporeDescription,MicroscopicDescription,Note")] Mushroom mushroom)
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
