using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class MushroomViewModel
    {
        public List<Mushroom>? Mushrooms { get; set; }
        public SelectList? Locations { get; set; }
        public SelectList? Families { get; set; }
        public string? MushroomLocation { get; set; }
        public string? MushroomFamily { get; set; }
        public bool? Edible { get; set; }
        public string? SearchString { get; set; }
    }
}
