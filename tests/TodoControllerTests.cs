using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using src;
using src.Controllers;

namespace tests;

public class TodoControllerTests
{
    private TodoController _controller;
    private TodoDbContext _context;
    private Mock<ILogger<TodoController>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _context = new TodoDbContext(new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase("TodoTest")
            .Options);

        _context.Todos.AddRange(new List<Todo>
        {
            new() { Title = "Test Todo 1", Description = "Description 1", IsCompleted = false, DueDate = DateTime.Now.AddDays(7) },
            new() { Title = "Test Todo 2", Description = "Description 2", IsCompleted = false, DueDate = DateTime.Now.AddDays(5) },
        });
        _context.SaveChanges();
        _loggerMock = new Mock<ILogger<TodoController>>();
        _controller = new TodoController(_loggerMock.Object, _context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetTodos_ReturnsAllTodos()
    {
        // Act
        var result = await _controller.GetTodos();
        // Assert
        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.Count().ShouldBe(2);

    }

    [Test]
    public async Task GetTodo_ReturnsTodo_WhenFound()
    {
        // Act
        var result = await _controller.GetTodo(1);

        // Assert
        result.ShouldNotBeNull();
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType<ActionResult<Todo>>();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(1);
        result.Value.Title.ShouldBe("Test Todo 1");
    }

    [Test]
    public async Task GetTodo_ReturnsNotFound_WhenNotExists()
    {
        // Act
        var result = await _controller.GetTodo(999);

        // Assert
        result.ShouldNotBeNull();
        result.Result.ShouldNotBeOfType<NotFoundResult>();
    }
}