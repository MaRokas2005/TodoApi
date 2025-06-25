using AutoMapper;
using Moq;
using Shouldly;
using Todo.Persistence.Entities;
using Todo.Persistence.Interfaces;
using Todo.Service;
using Todo.Service.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Todo.UnitTests.Service;

public class TodoServiceTests
{
    private Mock<ITodoRepository> _repoMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private TodoService _todoService = null!;
    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<ITodoRepository>();
        _mapperMock = new Mock<IMapper>();
        _todoService = new TodoService(_repoMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetAllTodosAsync_ReturnMappedTodos()
    {
        var todos = new List<TodoItem>
        {
            new(1, "TestTodo", "none", false, DateTime.Today.AddDays(2), DateTime.UtcNow, DateTime.UtcNow)
        };
        var mapped = new List<TodoDto>
        {
            new(1, "TestTodo", "none", false, DateTime.Today.AddDays(2))
        };

        _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(todos);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<TodoDto>>(todos)).Returns(mapped);

        var result = await _todoService.GetAllTodosAsync();

        result.ShouldNotBeNull();
        result.ShouldBe(mapped);
    }

    [Test]
    public async Task GetTodoByIdAsync_ReturnsMappedTodo_WhenFound()
    {
        var todo = new TodoItem(1, "TestTodo", "none", false, DateTime.Today.AddDays(2), DateTime.UtcNow, DateTime.UtcNow);
        var mapped = new TodoDto(1, "TestTodo", "none", false, DateTime.Today.AddDays(2));

        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todo);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(todo)).Returns(mapped);

        var result = await _todoService.GetTodoByIdAsync(1);

        result.ShouldNotBeNull();
        result.ShouldBe(mapped);
    }

    [Test]
    public async Task GetTodoByIdAsync_ReturnsMappedTodo_WhenNotFound()
    {
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem?)null);

        var result = await _todoService.GetTodoByIdAsync(1);

        result.ShouldBeNull();
    }

    [Test]
    public async Task CreateTodoAsync_ReturnsMappedCreatedTodo()
    {
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new CreateTodoDto("Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(0, "Get milk", "desc", false, tomorrow, now, now);
        var created = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);
        var resultDto = new TodoDto(1, "Get milk", "desc", false, tomorrow);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.AddAsync(entity)).ReturnsAsync(created);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(created)).Returns(resultDto);

        var result = await _todoService.CreateTodoAsync(dto);

        result.ShouldBe(resultDto);
    }

    [Test]
    public async Task UpdateTodoAsync_ReturnsMappedUpdatedTodo_WhenFound()
    {
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new UpdateTodoDto(1, "Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);
        var updated = new TodoItem(1, "Get milk", "Get milk from shop.", true, tomorrow, now, now);
        var resultDto = new TodoDto(1, "Get milk", "Get milk from shop.", true, tomorrow);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.UpdateAsync(entity)).ReturnsAsync(updated);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(updated)).Returns(resultDto);

        var result = await _todoService.UpdateTodoAsync(dto);

        result.ShouldBe(resultDto);
    }

    [Test]
    public async Task UpdateTodoAsync_ReturnsMappedUpdatedTodo_WhenNotFound()
    {
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new UpdateTodoDto(1, "Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.UpdateAsync(entity)).ReturnsAsync((TodoItem?)null);

        var result = await _todoService.UpdateTodoAsync(dto);

        result.ShouldBeNull();
    }

    [Test]
    public async Task DeleteTodoAsync_ReturnsTrue_WhenFound()
    {
        _repoMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _todoService.DeleteTodoAsync(1);

        result.ShouldBeTrue();
    }

    [Test]
    public async Task DeleteTodoAsync_ReturnsFalse_WhenNotFound()
    {
        _repoMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _todoService.DeleteTodoAsync(1);

        result.ShouldBeFalse();
    }
}
