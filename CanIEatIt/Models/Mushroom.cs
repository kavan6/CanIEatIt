using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class Mushroom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }
        public string? Location { get; set; }

        [Display(Name = "Cap Diameter (cm)")]
        public string? CapDiameter { get; set; }
        [Display(Name = "Stem Height (cm)")]
        public string? Height { get; set; }
        public bool? Edible { get; set; }
        [Display(Name = "Edible Description")]
        public string? EdibleDescription { get; set; }
        [Display(Name = "Cap Description")]
        public string? CapDescription { get; set; }
        [Display(Name = "Stem Description")]
        public string? StemDescription { get; set; }
        [Display(Name = "Gill Description")]
        public string? GillDescription { get; set; }
        [Display(Name = "Spore Description")]
        public string? SporeDescription { get; set; }
        [Display(Name = "Microscopic Description")]
        public string? MicroscopicDescription { get; set; }
        public string? Note { get; set; }

    }
}
