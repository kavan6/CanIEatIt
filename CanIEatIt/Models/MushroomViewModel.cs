using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class MushroomViewModel
    {
        public List<Mushroom>? Mushrooms { get; set; }

        // Drop down lists

        public SelectList? Families { get; set; }
        public SelectList? Locations { get; set; }
        public SelectList? Edibles { get; set; }

        //public SelectList? CapDiameters { get; set; }
        //public SelectList? StemHeights { get; set; }

        // Search variables
        public string? SearchName { get; set; }
        public string[]? SearchFamily { get; set; }
        public string? SearchLocation { get; set; }
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
        public string[]? SearchKeyWords { get; set; }

        public string? SearchImageUrl { get; set; }


    }
}
