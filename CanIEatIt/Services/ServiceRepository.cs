using Microsoft.AspNetCore.Mvc.Rendering;

namespace CanIEatIt.Services
{
    public interface IServiceRepository
    {
        Task<List<SelectListItem>> populateCapDiameters();

        Task<List<SelectListItem>> populateStemHeights();

        Task<List<SelectListItem>> populateLocations();
    }
    public class ServiceRepository : IServiceRepository
    {
        public Task<List<SelectListItem>> populateLocations()
        {
            return Task.Factory.StartNew(() =>
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "Africa" },
                    new SelectListItem() { Value = "1", Text = "Asia" },
                    new SelectListItem() { Value = "2", Text = "Europe" },
                    new SelectListItem() { Value = "3", Text = "North America" },
                    new SelectListItem() { Value = "4", Text = "Oceania" },
                    new SelectListItem() { Value = "5", Text = "South America" },
                };
            });
        }
        public Task<List<SelectListItem>> populateCapDiameters()
        {
            return Task.Factory.StartNew(() =>
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "0-2cm" },
                    new SelectListItem() { Value = "1", Text = "3-6cm" },
                    new SelectListItem() { Value = "2", Text = "7-14cm" },
                    new SelectListItem() { Value = "3", Text = "15-31cm" },
                    new SelectListItem() { Value = "4", Text = "32-64cm" },
                    new SelectListItem() { Value = "5", Text = "65-122cm" },
                    new SelectListItem() { Value = "6", Text = "123cm+" }
                };
            });
        }

        public Task<List<SelectListItem>> populateStemHeights()
        {
            return Task.Factory.StartNew(() =>
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "0-2cm" },
                    new SelectListItem() { Value = "1", Text = "3-6cm" },
                    new SelectListItem() { Value = "2", Text = "7-14cm" },
                    new SelectListItem() { Value = "3", Text = "15-31cm" },
                    new SelectListItem() { Value = "4", Text = "32-64cm" },
                    new SelectListItem() { Value = "5", Text = "65-122cm" },
                    new SelectListItem() { Value = "6", Text = "123cm+" }
                };
            });
        }
    }
}
