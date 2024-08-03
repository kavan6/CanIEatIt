namespace CanIEatIt.Models
{
    public class Mushroom
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }
        public string? Location { get; set; }
        public string? CapDiameter { get; set; }
        public string? Height { get; set; }
        public bool? Edible { get; set; }
        public string? EdibleDescription { get; set; }
        public string? CapDescription { get; set; }
        public string? StemDescription { get; set; }
        public string? GillDescription { get; set; }
        public string? SporeDescription { get; set; }
        public string? MicroscopicDescription { get; set; }
        public string? Note { get; set; }

    }
}
