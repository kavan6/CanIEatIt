using CanIEatIt.Controllers;
using CanIEatIt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CanIEatIt.UnitTests.Controllers;

public class HomeControllerTests
{
    private readonly ILogger<HomeController> _logger;
    [Fact]
    public void Index_ReturnsAView()
    {
        var controller = new HomeController(_logger);

        var result = (controller.Index()) as ViewResult;

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Null(viewResult.ViewData.Model);
    }
    [Fact]
    public void Privacy_ReturnsAView()
    {
        var controller = new HomeController(_logger);

        var result = (controller.Privacy()) as ViewResult;

        var viewResult = Assert.IsType<ViewResult>(result);
    }
    [Fact]
    public void Error_ReturnsAView()
    {
        // Set up mock http context
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.SetupGet(x => x.TraceIdentifier).Returns("test-trace-id");

        // Set up mock activity
        var mockActivity = new Activity("test");
        mockActivity.Start();
        Activity.Current = mockActivity;

        var controller = new HomeController(_logger);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };

        var result = (controller.Error()) as ViewResult;

        Assert.NotNull(result);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.NotNull(viewResult.ViewData.Model);
        var model = Assert.IsType<ErrorViewModel>(viewResult.ViewData.Model);
        Assert.False(string.IsNullOrEmpty(model.RequestId));
    }
}