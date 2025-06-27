using AutoMapper;
using Moq;
using Shouldly;
using Todo.Persistence.Entities;
using Todo.Persistence.Interfaces;
using Todo.Service;
using Todo.Service.Models;

namespace Todo.UnitTests.Service;

public class TodoServiceTests
{
    private Mock<ITodoRepository> _repoMock;
    private Mock<IMapper> _mapperMock;
    private TodoService _todoService;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<ITodoRepository>();
        _mapperMock = new Mock<IMapper>();
        _todoService = new TodoService(_repoMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetAllTodosAsync_GetsEntitiesFromRepo_ReturnMappedTodos()
    {
        // Arrange
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

        // Act
        var result = await _todoService.GetAllTodosAsync();

        // Assert
        _repoMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        result.ShouldBe(mapped);
    }

    [Test]
    public async Task GetTodoByIdAsync_GetsEntityFromRepo_ReturnsMappedTodo_WhenFound()
    {
        // Arrange
        var todo = new TodoItem(1, "TestTodo", "none", false, DateTime.Today.AddDays(2), DateTime.UtcNow, DateTime.UtcNow);
        var mapped = new TodoDto(1, "TestTodo", "none", false, DateTime.Today.AddDays(2));

        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todo);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(todo)).Returns(mapped);

        // Act
        var result = await _todoService.GetTodoByIdAsync(1);

        // Assert
        _repoMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        result.ShouldBe(mapped);
    }

    [Test]
    public async Task GetTodoByIdAsync_AttemptsToGetEntityFromRepo_ReturnsNull_WhenNotFound()
    {
        // Arrange
        _repoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem?)null);

        // Act
        var result = await _todoService.GetTodoByIdAsync(1);

        // Assert
        _repoMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        result.ShouldBeNull();
    }

    [Test]
    public async Task CreateTodoAsync_AddsEntityToRepo_ReturnsMappedCreatedTodo()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new CreateTodoDto("Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(0, "Get milk", "desc", false, tomorrow, now, now);
        var created = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);
        var resultDto = new TodoDto(1, "Get milk", "desc", false, tomorrow);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.AddAsync(entity)).ReturnsAsync(created);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(created)).Returns(resultDto);

        // Act
        var result = await _todoService.CreateTodoAsync(dto);

        // Assert
        _repoMock.Verify(repo => repo.AddAsync(entity), Times.Once);
        result.ShouldBe(resultDto);
    }

    [Test]
    public async Task UpdateTodoAsync_UpdatesEntityInRepo_ReturnsMappedUpdatedTodo_WhenFound()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new UpdateTodoDto(1, "Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);
        var updated = new TodoItem(1, "Get milk", "Get milk from shop.", true, tomorrow, now, now);
        var resultDto = new TodoDto(1, "Get milk", "Get milk from shop.", true, tomorrow);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.UpdateAsync(entity)).ReturnsAsync(updated);
        _mapperMock.Setup(mapper => mapper.Map<TodoDto>(updated)).Returns(resultDto);

        // Act
        var result = await _todoService.UpdateTodoAsync(dto);

        // Assert
        _repoMock.Verify(repo => repo.UpdateAsync(entity), Times.Once);
        result.ShouldBe(resultDto);
    }

    [Test]
    public async Task UpdateTodoAsync_AttemptsToUpdateEntityInRepo_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var tomorrow = DateTime.Today.AddDays(1);
        var dto = new UpdateTodoDto(1, "Get milk", "desc", false, tomorrow);
        var entity = new TodoItem(1, "Get milk", "desc", false, tomorrow, now, now);

        _mapperMock.Setup(mapper => mapper.Map<TodoItem>(dto)).Returns(entity);
        _repoMock.Setup(repo => repo.UpdateAsync(entity)).ReturnsAsync((TodoItem?)null);

        // Act
        var result = await _todoService.UpdateTodoAsync(dto);

        // Assert
        _repoMock.Verify(repo => repo.UpdateAsync(entity), Times.Once);
        result.ShouldBeNull();
    }

    [Test]
    public async Task DeleteTodoAsync_ReturnsTrue_WhenFound()
    {
        // Arrange
        _repoMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _todoService.DeleteTodoAsync(1);

        // Assert
        _repoMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        result.ShouldBeTrue();
    }

    [Test]
    public async Task DeleteTodoAsync_ReturnsFalse_WhenNotFound()
    {
        // Arrange
        _repoMock.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _todoService.DeleteTodoAsync(1);

        // Assert
        _repoMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        result.ShouldBeFalse();
    }
}
