using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CanIEatIt.Data;
using CanIEatIt.Models;
using CanIEatIt.Services;
using System.Text.RegularExpressions;

namespace CanIEatIt.Controllers
{

    public class MushroomsController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        private readonly CanIEatItContext _context;

        Regex regexFirstInteger = new Regex(@"(\d+)-");
        Regex regexSecondInteger = new Regex(@"(\d+)c|C");
        Regex intMatcher = new Regex(@"(\d+)");

        public MushroomsController(CanIEatItContext context, IServiceRepository services)
        {
            _context = context;
            _serviceRepository = services;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<string> familyQuery = from m in _context.Mushroom orderby m.Family select m.Family;

            var mushrooms = from m in _context.Mushroom select m;

            var mushroomEdibleVM = new MushroomViewModel
            {
                Locations = new SelectList(await _serviceRepository.populateLocations(), "Value", "Text"),
                Families = new SelectList(await familyQuery.Distinct().ToListAsync()),
                Edibles = new SelectList(await _serviceRepository.populateEdible(), "Value", "Text"),
                Mushrooms = await mushrooms.ToListAsync()
            };

            return View(mushroomEdibleVM);
        }

        public IActionResult Picking()
        {
            return View();
        }
        public IActionResult Recipes()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        // GET: AJAX async search

        public async Task<IActionResult> Search(string searchValue)
        {
            if (searchValue == "Search mushrooms...")
            {
                searchValue = null;
            }

            if (_context.Mushroom == null)
            {
                return Problem("Entity set 'MushroomContext.Mushroom' is null.");
            }

            var mushrooms = from m in _context.Mushroom select m;

            if (!string.IsNullOrEmpty(searchValue))
            {
                mushrooms = mushrooms.Where(x => x.Name!.ToUpper().Contains(searchValue.ToUpper()));
            }

            var results = await mushrooms.ToListAsync();

            var mushroomEdibleVM = new MushroomViewModel
            {
                Mushrooms = results,
                SearchName = searchValue
           
            };

            return Json(mushroomEdibleVM);

        }

        // GET: Main Page loader
        [HttpGet]
        public async Task<IActionResult> Database(
                                                  string searchName, string[] searchFamily, string[] searchLocation,
                                                  int? searchCapDiameter, int? searchStemHeight, string searchEdible, 
                                                  string[] searchKeyWords
                                                 )
        {

            //if(searchName == "Name...")
            //{
            //    searchName = null;
            //}



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

            if (searchCapDiameter.HasValue && searchStemHeight > 0)
            {
                mushrooms = mushrooms.Where(x => ((searchCapDiameter >= x.LowerDiameter)&&(searchCapDiameter <= x.UpperDiameter)));
            }

            if (searchStemHeight.HasValue && searchStemHeight > 0)
            {
                mushrooms = mushrooms.Where(x => ((searchStemHeight >= x.LowerHeight)&&(searchStemHeight <= x.UpperHeight)));
            }

            if (!string.IsNullOrEmpty(searchEdible))
            {
                switch (searchEdible)
                {
                    case "0":
                        mushrooms = mushrooms.Where(x => x.Edible);
                        break;
                    case "1":
                        mushrooms = mushrooms.Where(x => !x.Edible);
                        break;
                    default:
                        break;
                }
            }

            //if (!string.IsNullOrEmpty(searchCapDes))
            //{
            //    mushrooms = mushrooms.Where(x => x.CapDescription!.ToUpper().Contains(searchCapDes.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(searchStemDes))
            //{
            //    mushrooms = mushrooms.Where(x => x.StemDescription!.ToUpper().Contains(searchStemDes.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(searchGillDes))
            //{
            //    mushrooms = mushrooms.Where(x => x.GillDescription!.ToUpper().Contains(searchGillDes.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(searchSporeDes))
            //{
            //    mushrooms = mushrooms.Where(x => x.SporeDescription!.ToUpper().Contains(searchSporeDes.ToUpper()) || x.MicroscopicDescription!.ToUpper().Contains(searchSporeDes.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(searchNote))
            //{
            //    mushrooms = mushrooms.Where(x => x.Note!.ToUpper().Contains(searchNote.ToUpper()));
            //}

            if (searchKeyWords.Count() > 0)
            {
                foreach (var word in searchKeyWords)
                {
                    mushrooms = mushrooms.Where(
                        x => 
                    (x.Name!.ToUpper().Contains(word.ToUpper())) ||
                    (x.Family!.ToUpper().Contains(word.ToUpper())) ||
                    (x.Location!.ToUpper().Contains(word.ToUpper())) ||
                    (x.EdibleDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.CapDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.StemDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.GillDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.SporeDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.MicroscopicDescription!.ToUpper().Contains(word.ToUpper())) ||
                    (x.Note!.ToUpper().Contains(word.ToUpper()))
                    );
                }
            }


            // IN MEMORY OPERATIONS

            var allMushrooms = await mushrooms.ToListAsync();

            if (searchLocation.Count() > 0)
            {
                if (!searchLocation.Contains("All"))
                {
                    allMushrooms = allMushrooms.Where(x => searchLocation.Any(loc => x.Location.Contains(loc, StringComparison.OrdinalIgnoreCase))).ToList();
                }
            }

            if (searchFamily.Count() > 0)
            {
                if (!searchFamily.Contains("All"))
                {
                    var families = searchFamily.Where(fam => fam != null);

                    allMushrooms = allMushrooms.Where(x => families.Any(fam => x.Family.Contains(fam, StringComparison.OrdinalIgnoreCase))).ToList();
                }
            }

            #endregion

            var mushroomEdibleVM = new MushroomViewModel
            {
                Locations = new SelectList(await _serviceRepository.populateLocations(), "Value", "Text"),
                Families = new SelectList(await familyQuery.Distinct().ToListAsync()),
                Edibles = new SelectList(await _serviceRepository.populateEdible(), "Value", "Text"),
                Mushrooms = allMushrooms,
                SearchName = searchName,
                SearchFamily = searchFamily
                
            };

            return View(mushroomEdibleVM);
        }

        // GET: Mushrooms/Information/ID
        public async Task<IActionResult> Information(int? id)
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
                // Set the upper and lower diameter and height for created mushroom from their respective ranges. Ranges come in the form of [0-9]-[0-9]cm.

                string n1 = intMatcher.Match(regexFirstInteger.Match(mushroom.CapDiameter!).ToString()).ToString();
                string n2 = intMatcher.Match(regexSecondInteger.Match(mushroom.CapDiameter!).ToString()).ToString();

                int n1res = int.Parse(n1);
                int n2res = int.Parse(n2);

                mushroom.LowerDiameter = n1res;
                mushroom.UpperDiameter = n2res;

                n1 = intMatcher.Match(regexFirstInteger.Match(mushroom.StemHeight!).ToString()).ToString();
                n2 = intMatcher.Match(regexSecondInteger.Match(mushroom.StemHeight!).ToString()).ToString();

                n1res = int.Parse(n1);
                n2res = int.Parse(n2);

                mushroom.LowerHeight = n1res;
                mushroom.UpperHeight = n2res;

                Directory.CreateDirectory("wwwroot/images/Mushrooms/" + mushroom.Name);

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
                    // need to set hidden fields upon edit

                    string n1 = intMatcher.Match(regexFirstInteger.Match(mushroom.CapDiameter!).ToString()).ToString();
                    string n2 = intMatcher.Match(regexSecondInteger.Match(mushroom.CapDiameter!).ToString()).ToString();

                    int n1res = int.Parse(n1);
                    int n2res = int.Parse(n2);

                    mushroom.LowerDiameter = n1res;
                    mushroom.UpperDiameter = n2res;

                    n1 = intMatcher.Match(regexFirstInteger.Match(mushroom.StemHeight!).ToString()).ToString();
                    n2 = intMatcher.Match(regexSecondInteger.Match(mushroom.StemHeight!).ToString()).ToString();

                    n1res = int.Parse(n1);
                    n2res = int.Parse(n2);

                    mushroom.LowerHeight = n1res;
                    mushroom.UpperHeight = n2res;

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
