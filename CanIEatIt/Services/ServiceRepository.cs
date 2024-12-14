﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace CanIEatIt.Services
{
    public interface IServiceRepository
    {
        Task<List<SelectListItem>> populateCapDiameters();

        Task<List<SelectListItem>> populateStemHeights();

        Task<List<SelectListItem>> populateLocations();

        Task<List<SelectListItem>> populateEdible();
    }
    public class ServiceRepository : IServiceRepository
    {
        public async Task<List<SelectListItem>> populateLocations()
        {
            return await Task.Factory.StartNew(() =>
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "Africa", Text = "Africa" },
                    new SelectListItem() { Value = "Asia", Text = "Asia" },
                    new SelectListItem() { Value = "Europe", Text = "Europe" },
                    new SelectListItem() { Value = "North America", Text = "North America" },
                    new SelectListItem() { Value = "Oceania", Text = "Oceania" },
                    new SelectListItem() { Value = "South America", Text = "South America" },
                };
            });
        }

        public async Task<List<SelectListItem>> populateEdible()
        {
            return await Task.Factory.StartNew(() =>
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() { Value = "0", Text = "Yes" },
                    new SelectListItem() { Value = "1", Text = "No" },
                };
            });
        }
        public async Task<List<SelectListItem>> populateCapDiameters()
        {
            return await Task.Factory.StartNew(() =>
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

        public async Task<List<SelectListItem>> populateStemHeights()
        {
            return await Task.Factory.StartNew(() =>
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
