using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CanIEatIt.Models
{
    public class Mushroom
    {
        public int Id { get; set; }

        [StringLength(255, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        [StringLength(255, MinimumLength = 3)]
        [Required]
        public string? Family { get; set; }
        [Required]
        public string? Location { get; set; }

        [RegularExpression(@"^[0-9]+-[0-9]+cm")]
        [Display(Name = "Cap Diameter (cm)")]
        public string? CapDiameter { get; set; }
        [HiddenInput]
        public int? LowerDiameter { get; set; }
        [HiddenInput]
        public int? UpperDiameter { get; set; }

        [RegularExpression(@"^[0-9]+-[0-9]+cm")]
        [Display(Name = "Stem Height (cm)")]
        public string? StemHeight { get; set; }
        [HiddenInput]
        public int? LowerHeight { get; set; }
        [HiddenInput]
        public int? UpperHeight { get; set; }

        [Required]
        public bool Edible { get; set; }
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
