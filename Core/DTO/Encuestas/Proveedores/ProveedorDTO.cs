using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class ProveedorDTO
    {
        public int Numero { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public string Ubicacion { get; set; }
        public int TipoMoneda { get; set; }
        public int Tipo { get; set; }
        public DateTime FechaAntiguedad { get; set; }
    }
}