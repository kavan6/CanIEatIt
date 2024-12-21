using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class MushroomViewModel
    {
        public List<Mushroom>? Mushrooms { get; set; }

        // Drop down lists

        public SelectList? Families { get; set; }
        public List<String>? SearchFamilies { get; set; }
        public SelectList? Locations { get; set; }

        public List<String>? SearchLocations { get; set; }
        public SelectList? Edibles { get; set; }

        public List<String>? SearchKeywords {  get; set; }

        // Search variables
        public string? SearchName { get; set; }
        public string? SearchCapDiameter { get; set; }
        public string? SearchStemHeight { get; set; }
        public string? SearchEdible { get; set; }
        public string? SearchEdibleDes { get; set; }
        public string? SearchCapDes { get; set; }
        public string? SearchStemDes { get; set; }
        public string? SearchGillDes { get; set; }
        public string? SearchSporeDes { get; set; }
        public string? SearchMicroDes { get; set; }
        public string? SearchNote { get; set; }

        public List<string>? ImageURLS { get; set; }


    }
}
