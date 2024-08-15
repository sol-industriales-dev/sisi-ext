using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TipoUsuario { get; set; }
        public int EmpleadoId { get; set; }
        public int? JefeId { get; set; }
        public bool esAdmin { get; set; }
        public bool VerComoActivado { get; set; }
    }
}