using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class Respuesta
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
        
        public Respuesta()
        {
            Message = "Error: ";
        }
    }
}