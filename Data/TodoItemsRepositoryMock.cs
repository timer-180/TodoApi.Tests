using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Tests.Data
{
    internal class TodoItemsRepositoryMock : ITodoItemsRepository
    {
        private List<TodoItem> _todoItems;

        public TodoItemsRepositoryMock(List<TodoItem> todoItems)
        {
            _todoItems = todoItems;
        }

        public Task<List<TodoItem>> GetTodoItems()
        {
            return Task.FromResult(_todoItems);
        }

        public Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTodoItem(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TodoItem?> GetTodoItem(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TodoItem?> SwitchTodoItem(long id)
        {
            throw new NotImplementedException();
        }
    }
}
