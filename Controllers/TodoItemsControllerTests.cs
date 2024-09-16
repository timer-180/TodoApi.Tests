using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Controllers
{
    public class TodoItemsControllerTests
    {
        private static readonly DateTime s_creationDateTime = DateTime.Now;

        private readonly Mock<ILogger<TodoItemsController>> _loggerMock = new();

        [Fact]
        public async Task GetTodoItems_Ok()
        {
            Mock<ITodoItemsRepository> repositoryMock = new();
            List<TodoItem> todoItems = [GetMockedTodoItem(1), GetMockedTodoItem(2)];
            repositoryMock.Setup(r => r.GetTodoItems()).Returns(Task.FromResult(todoItems));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.GetTodoItems();

            Assert.NotNull(result.Result);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.True(objectResult is OkObjectResult);
            Assert.True(objectResult.Value is List<TodoItem>);
            Assert.Equal(todoItems.Count, ((List<TodoItem>)objectResult.Value).Count());
        }

        [Fact]
        public async Task GetTodoItems_Error()
        {
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetTodoItems()).Throws(new NullReferenceException());

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.GetTodoItems();

            CheckResultCode(result, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetTodoItem_Ok()
        {
            long id = 1;
            TodoItem todoItem = GetMockedTodoItem(id);
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetTodoItem(id)).Returns(Task.FromResult<TodoItem?>(todoItem));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.GetTodoItem(id);

            Assert.NotNull(result.Result);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.True(objectResult is OkObjectResult);
            Assert.True(objectResult.Value is TodoItem);
            Assert.Equal(id, ((TodoItem)objectResult.Value).Id);
        }

        [Fact]
        public async Task GetTodoItem_NotFound()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetTodoItem(id)).Returns(Task.FromResult<TodoItem?>(null));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.GetTodoItem(id);

            CheckResultCode(result, StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetTodoItem_Error()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.GetTodoItem(id)).Throws(new NullReferenceException());

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.GetTodoItem(id);

            CheckResultCode(result, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task CreateTodoItem_Ok()
        {
            long id = 1;
            TodoItem todoItem = GetMockedTodoItem(id);
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.CreateTodoItem(todoItem)).Returns(Task.FromResult(todoItem));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.CreateTodoItem(todoItem);

            Assert.NotNull(result.Result);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.True(objectResult is CreatedAtActionResult);
            Assert.True(objectResult.Value is TodoItem);
            Assert.Equal(id, ((TodoItem)objectResult.Value).Id);
        }

        [Fact]
        public async Task CreateTodoItem_Error()
        {
            TodoItem todoItem = GetMockedTodoItem(1);
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.CreateTodoItem(todoItem)).Throws(new NullReferenceException());

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.CreateTodoItem(todoItem);

            CheckResultCode(result, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task SwitchTodoItem_Ok()
        {
            long id = 1;
            TodoItem todoItemInit = GetMockedTodoItem(id);
            TodoItem todoItemSwitched = GetMockedTodoItem(id);
            todoItemSwitched.IsComplete = !todoItemInit.IsComplete;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.SwitchTodoItem(id)).Returns(Task.FromResult<TodoItem?>(todoItemSwitched));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.SwitchTodoItem(id);

            Assert.NotNull(result.Result);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.True(objectResult is OkObjectResult);
            Assert.True(objectResult.Value is TodoItem);
            Assert.NotEqual(todoItemInit.IsComplete, ((TodoItem)objectResult.Value).IsComplete);
        }

        [Fact]
        public async Task SwitchTodoItem_NotFound()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.SwitchTodoItem(id)).Returns(Task.FromResult<TodoItem?>(null));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.SwitchTodoItem(id);

            CheckResultCode(result, StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task SwitchTodoItem_Error()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.SwitchTodoItem(id)).Throws(new NullReferenceException());

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.SwitchTodoItem(id);

            CheckResultCode(result, StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task DeleteTodoItem_Ok()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.DeleteTodoItem(id)).Returns(Task.FromResult(true));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.DeleteTodoItem(id);

            Assert.NotNull(result);
            Assert.True(result is NoContentResult);
        }

        [Fact]
        public async Task DeleteTodoItem_NotFound()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.DeleteTodoItem(id)).Returns(Task.FromResult(false));

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.DeleteTodoItem(id);

            Assert.NotNull(result);
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async Task DeleteTodoItem_Error()
        {
            long id = 1;
            Mock<ITodoItemsRepository> repositoryMock = new();
            repositoryMock.Setup(r => r.DeleteTodoItem(id)).Throws(new NullReferenceException());

            TodoItemsController controller = new(_loggerMock.Object, repositoryMock.Object);
            var result = await controller.DeleteTodoItem(id);

            Assert.NotNull(result);
            Assert.True(result is StatusCodeResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

/*
        [Fact]
        public async Task GetTodoItemsTest1()
        {
            Mock<ILogger<TodoItemsController>> loggerMock = new();

            ITodoItemsRepository repositoryMock = new TodoItemsRepositoryMock(GetMockedTodoItems());

            TodoItemsController controller = new(loggerMock.Object, repositoryMock);
            var result = await controller.GetTodoItems();

            Assert.NotNull(result.Result);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.True(objectResult is OkObjectResult);
            Assert.True(objectResult.Value is List<TodoItem>);
            Assert.Equal(2, ((List<TodoItem>)objectResult.Value).Count());
        }
*/
        private static void CheckResultCode<TValue>(ActionResult<TValue> result, int expectedCode)
        {
            Assert.NotNull(result.Result);
            StatusCodeResult statusResult = (StatusCodeResult)result.Result;
            Assert.Equal(expectedCode, statusResult.StatusCode);
        }

        private static TodoItem GetMockedTodoItem(long id)
        {
            return new()
            {
                Id = id,
                Title = $"Title{id}",
                Created = s_creationDateTime,
                IsComplete = false,
            };
        }
    }
}
