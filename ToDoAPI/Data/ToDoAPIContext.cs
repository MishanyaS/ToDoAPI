using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;

namespace ToDoAPI.Data
{
    public class ToDoAPIContext : DbContext
    {
        public ToDoAPIContext (DbContextOptions<ToDoAPIContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoAPI.Models.ToDoItem> ToDoItem { get; set; }
    }
}
