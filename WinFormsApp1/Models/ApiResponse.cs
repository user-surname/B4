using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1.Models
{
    public class ApiResponse<T>
    {
        public int coderror { get; set; }
        public string action { get; set; }
        public string msg { get; set; }
        public DateTime ts { get; set; }
        public long exectimems { get; set; }
        public int count { get; set; }
        public T data { get; set; }  // Aquí va directamente la lista
    }
}
