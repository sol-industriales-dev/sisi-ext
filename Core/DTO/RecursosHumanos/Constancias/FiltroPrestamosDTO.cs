using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class FiltroPrestamosDTO
    {
        public List<string> lstCC { get; set; }
        public string estatus { get; set; }
        public string tipoPrestamo { get; set; }
        public int cantidad { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public string cc { get; set; }
        public DateTime fecha_creacion { get; set; }
    }
}
