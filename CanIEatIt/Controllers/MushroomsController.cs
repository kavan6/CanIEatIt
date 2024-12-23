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
using NUglify.Helpers;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;

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

            var locations = await _serviceRepository.populateLocations();
            var families = await familyQuery.Distinct().ToListAsync();
            var edibles = await _serviceRepository.populateEdible();

            var mushroomEdibleVM = new MushroomViewModel
            {
                Locations = new SelectList(locations ?? new List<SelectListItem>(), "Value", "Text"),
                Families = new SelectList(families ?? new List<string>()),
                Edibles = new SelectList(edibles ?? new List<SelectListItem>(), "Value", "Text"),
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

        // GET: Sort by alphabetical
        public async Task<IActionResult> SortAlphabetical(List<Mushroom> mushrooms)
        {
            // If there are enough mushrooms to sort, sort them :)
            if (mushrooms != null && mushrooms.Count > 1)
            {
                mushrooms = mushrooms.OrderBy(m => m.Name).ToList();
            }

            var mushroomVM = new MushroomViewModel { Mushrooms = mushrooms };
            return Json(mushroomVM);
        }
        public List<Mushroom> SortAlphabeticalMushrooms(List<Mushroom> mushrooms)
        {
            // If there are enough mushrooms to sort, sort them :)
            if (mushrooms != null && mushrooms.Count > 1)
            {
                mushrooms = mushrooms.OrderBy(m => m.Name).ToList();
            }
            return mushrooms;
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

            var URLS = results.Select(m => new
            {
                url = GetImageURL(m.Name!)
            }).Select(a => a.url).ToList();

            var mushroomEdibleVM = new MushroomViewModel
            {
                Mushrooms = results,
                SearchName = searchValue,
                ImageURLS = URLS
           
            };

            return Json(mushroomEdibleVM);

        }

        public string GetImageURL(string mushroomName)
        {
            var dir = Path.Combine("wwwroot", "images", "Mushrooms", mushroomName);
            if (!Directory.Exists(dir)) return "/images/default.png";

            var files = Directory.GetFiles(dir);

            if(files.Any())
            {
                return "/images/Mushrooms/" + mushroomName + "/" + Path.GetFileName(files.First());
            }
            return "/images/default.png";
        }

        // GET: Main Page loader
        [HttpGet]
        public async Task<IActionResult> Database(
                                                  List<String> searchFamily, List<String> searchLocation,
                                                  int? searchCapDiameter, int? searchStemHeight, string searchEdible, 
                                                  List<String> searchKeyWords
                                                 )
        {



            if (_context.Mushroom == null)
            {
                return Problem("Entity set 'MushroomContext.Mushroom' is null.");
            }

            IQueryable<string> familyQuery = from m in _context.Mushroom orderby m.Family select m.Family;

            var mushrooms = from m in _context.Mushroom select m;

            #region LINQ Searches

            if (searchCapDiameter.HasValue && searchCapDiameter > 0)
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
                SearchCapDiameter = searchCapDiameter.ToString(),
                SearchStemHeight = searchStemHeight.ToString(),
                SearchLocations = searchLocation?.ToList(),
                SearchFamilies = searchFamily?.ToList(),
                SearchKeywords = searchKeyWords?.ToList()
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
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mushrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([Bind("Id,Name,Family,Location,CapDiameter,StemHeight,Edible,EdibleDescription,CapDescription,StemDescription,GillDescription,SporeDescription,MicroscopicDescription,Note")] Mushroom mushroom, List<IFormFile> files)
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

                await UploadFiles(mushroom.Name!, files);

                _context.Add(mushroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Database));
            }
            return View(mushroom);
        }

        private readonly List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UploadFiles(string mName, List<IFormFile> files)
        {
            string baseDirectory = Path.Combine("wwwroot", "images", "Mushrooms", mName);
            Directory.CreateDirectory(baseDirectory);

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    var ext = Path.GetExtension(formFile.FileName).ToLowerInvariant();

                    if(!allowedExtensions.Contains(ext))
                    {
                        ModelState.AddModelError("File", $"The file {formFile.FileName} is not a valid image file. Only .jpg, .jpeg, and .png are allowed.");
                        return Forbid();
                    }

                    var filePath = Path.Combine(baseDirectory, Path.GetFileName(formFile.FileName));

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count(), size });
        }

		// GET: Mushrooms/Edit/5
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
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
