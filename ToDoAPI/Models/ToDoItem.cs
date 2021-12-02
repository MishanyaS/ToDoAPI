using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAPI.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string WhatToDo { get; set; }
        public DateTime Time { get; set; }
        public bool IsDone { get; set; }
        public string HiddenInfo { get; set; }
    }
}
