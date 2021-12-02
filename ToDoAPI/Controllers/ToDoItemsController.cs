using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Dtos;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoAPIContext _context;

        public ToDoItemsController(ToDoAPIContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetTodoItems()
        {
            return await _context.ToDoItem
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDTO>> GetTodoItem(int id)
        {
            var todoItem = await _context.ToDoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, ToDoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.ToDoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.WhatToDo = todoItemDTO.WhatToDo;
            todoItem.Time = todoItemDTO.Time;
            todoItem.IsDone = todoItemDTO.IsDone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemDTO>> CreateTodoItem(ToDoItemDTO todoItemDTO)
        {
            var todoItem = new ToDoItem
            {
                WhatToDo = todoItemDTO.WhatToDo,
                Time = todoItemDTO.Time,
                IsDone = todoItemDTO.IsDone
        };

            _context.ToDoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.ToDoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id) =>
             _context.ToDoItem.Any(e => e.Id == id);

        private static ToDoItemDTO ItemToDTO(ToDoItem todoItem) =>
            new ToDoItemDTO
            {
                Id = todoItem.Id,
                WhatToDo = todoItem.WhatToDo,
                Time = todoItem.Time,
                IsDone = todoItem.IsDone
            };
    }
}
