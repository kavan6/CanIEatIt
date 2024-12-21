﻿using CanIEatIt.Controllers;
using CanIEatIt.Data;
using CanIEatIt.Models;
using CanIEatIt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CanIEatIt.UnitTests.Controllers;

public class MushroomsControllerTests
{
    
    public Mock<IServiceRepository> SetupMockRepo()
    {
        var mockRepository = new Mock<IServiceRepository>();

        mockRepository.Setup(repo => repo.populateLocations())
        .ReturnsAsync(new List<SelectListItem>
        {
                    new SelectListItem() { Value = "Africa", Text = "Africa" },
                    new SelectListItem() { Value = "Asia", Text = "Asia" }
        });

        mockRepository.Setup(repo => repo.populateEdible())
        .ReturnsAsync(new List<SelectListItem>
        {
            new SelectListItem() { Value = "0", Text = "Yes" },
            new SelectListItem() { Value = "1", Text = "No" }
        });

        return mockRepository;
    }

    public CanIEatItContext SetupMockNullMushroomContext()
    {
        // Arrange: Set up the in-memory database
        var options = new DbContextOptionsBuilder<CanIEatItContext>()
            .UseInMemoryDatabase("TestDatabase") // Name the in-memory DB for isolation
            .Options;

        var context = new CanIEatItContext(options);

        context.Mushroom = null;
        context.SaveChanges();

        return context;
    }

    public CanIEatItContext SetupMockEmptyContext()
    {
        // Arrange: Set up the in-memory database
        var options = new DbContextOptionsBuilder<CanIEatItContext>()
            .UseInMemoryDatabase("TestDatabase") // Name the in-memory DB for isolation
            .Options;

        var context = new CanIEatItContext(options);

        context.SaveChanges();

        return context;
    }

    public CanIEatItContext SetupMockNormalContext(string dbName, int nMushrooms)
    {
        // Arrange: Set up the in-memory database
        var options = new DbContextOptionsBuilder<CanIEatItContext>()
            .UseInMemoryDatabase(dbName) // Name the in-memory DB for isolation
            .Options;

        var context = new CanIEatItContext(options);

        // Add some test data to the in-memory database
        for (int i = 0; i < nMushrooms; i++)
        {
            context.Mushroom.Add(new Mushroom { Name = "Agaricus", Family = "Agaricaceae", Location = i +"Europe", Edible = true });
        }
        context.SaveChanges();

        return context;
    }

    public CanIEatItContext SetupMockUnorderedContext(string dbName)
    {
        // Arrange: Set up the in-memory database
        var options = new DbContextOptionsBuilder<CanIEatItContext>()
            .UseInMemoryDatabase(dbName) // Name the in-memory DB for isolation
            .Options;

        var context = new CanIEatItContext(options);

        // Add some test data to the in-memory database
        context.Mushroom.Add(new Mushroom { Name = "Coprinellus", Family = "Coprine", Location = "Europe", Edible = true });
        context.Mushroom.Add(new Mushroom { Name = "Boletus", Family = "Bolete", Location = "Europe", Edible = true });
        context.Mushroom.Add(new Mushroom { Name = "Agaricus", Family = "Agaricaceae", Location = "Europe", Edible = true });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithAListOfMushrooms()
    {
        var mockRepository = SetupMockRepo();

        var nMushrooms = 2;
        var context = SetupMockNormalContext("ListOfMushrooms", 2);

        var controller = new MushroomsController(context, mockRepository.Object);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);

        var model = Assert.IsType<MushroomViewModel>(viewResult.ViewData.Model);

        Assert.Equal(nMushrooms, model.Mushrooms.Count());
    }

    [Fact]
    public async Task Search_ReturnsEveryMushroom_WhenNullOrSpecific()
    {
        var mockRepository = SetupMockRepo();

        var nMushrooms = 2;
        var context = SetupMockNormalContext("NullOrSpecific", nMushrooms);

        var controller = new MushroomsController(context, mockRepository.Object);

        // Search value that is automatically populated in the search box
        var searchValue = "Search mushrooms...";

        var result = await controller.Search(searchValue);

        var jsonResult = Assert.IsType<JsonResult>(result);

        var mushrooms = ((dynamic)jsonResult.Value).Mushrooms;

        Assert.Equal(nMushrooms, mushrooms.Count);
    }

    [Fact]
    public async Task Search_ReturnsProblem_WhenContextIsNull()
    {
        var mockRepository = SetupMockRepo();

        var context = SetupMockNullMushroomContext();

        var controller = new MushroomsController(context, mockRepository.Object);

        // Arbitrary search value
        var searchValue = "Agaricus";

        var result = await controller.Search(searchValue);

        var objectResult = Assert.IsType<ObjectResult>(result);

        var detail = ((dynamic)objectResult.Value).Detail;

        Assert.Equal("Entity set 'MushroomContext.Mushroom' is null.", detail);
    }

    [Fact]
    public async Task Search_ReturnsExpectedResult_WhenValueIsGood()
    {
        var mockRepository = SetupMockRepo();

        var nMushrooms = 1;
        var context = SetupMockNormalContext("ValueIsGood", nMushrooms);

        var controller = new MushroomsController(context, mockRepository.Object);

        // Arbitrary search value
        var searchValue = "Agaricus";

        var result = await controller.Search(searchValue);

        var jsonResult = Assert.IsType<JsonResult>(result);

        var dynamicRes = ((dynamic)jsonResult.Value);
        var mushrooms = dynamicRes.Mushrooms;
        var searchValRes = dynamicRes.SearchName;
        var URLS = dynamicRes.ImageURLS;

        Assert.Equal(1, mushrooms.Count);
        Assert.Equal("Agaricus", mushrooms[0].Name);
        Assert.Equal(searchValue, searchValRes);
        Assert.Equal("/images/default.png", URLS[0]);
    }

    [Fact]
    public async Task SortAlphabetical_ReturnsAlphabeticallySortedList()
    {
        var mockRepository = SetupMockRepo();

        var nMushrooms = 3;
        var context = SetupMockUnorderedContext("Alphabetical");

        var controller = new MushroomsController(context, mockRepository.Object);

        // Arbitrary search value
        var preMushrooms = context.Mushroom.ToList();

        var result = await controller.SortAlphabetical(preMushrooms);

        var jsonResult = Assert.IsType<JsonResult>(result);

        var mushrooms = ((dynamic)jsonResult.Value).Mushrooms;

        Assert.Equal("Coprinellus", preMushrooms[0].Name);
        Assert.Equal("Boletus", preMushrooms[1].Name);
        Assert.Equal("Agaricus", preMushrooms[2].Name);

        Assert.Equal("Agaricus", mushrooms[0].Name);
        Assert.Equal("Boletus", mushrooms[1].Name);
        Assert.Equal("Coprinellus", mushrooms[2].Name);
    }

    [Fact]
    public async Task Information_ReturnsNotFound_WhenIDIsNul()
    {
        var mockRepository = SetupMockRepo();

        var mockContext = SetupMockNullMushroomContext();

        var controller = new MushroomsController(mockContext, mockRepository.Object);

        int? id = null;
        var result = await controller.Information(id);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Information_ReturnsNotFound_WhenMushroomsEmpty()
    {
        var mockRepository = SetupMockRepo();

        var mockContext = SetupMockEmptyContext();

        var controller = new MushroomsController(mockContext, mockRepository.Object);

        int? id = 0;
        var result = await controller.Information(id);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Information_ReturnsMushroomView_WhenIDIsFound()
    {
        var mockRepository = SetupMockRepo();

        var mockContext = SetupMockUnorderedContext("IDFound");

        var controller = new MushroomsController(mockContext, mockRepository.Object);

        int? id = 1;
        var result = await controller.Information(id);

        var viewResult = Assert.IsType<ViewResult>(result);

        var mushroom = Assert.IsType<Mushroom>(viewResult.ViewData.Model);
    }

}