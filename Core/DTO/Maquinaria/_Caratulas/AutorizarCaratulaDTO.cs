using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class AutorizarCaratulaDTO
    {
        public int id { get; set; }
        public int idCaratula { get; set; }       
        public bool esAutorizado { get; set; }
        public string firma { get; set; }
        public DateTime fechaAutorizacion { get; set; }
        public string comentario { get; set; }
        public int claveAutorizante { get; set; }
        public string UsuarioTecnico { get; set; }
        public string UsuarioServicio { get; set; }
        public string UsuarioConstruccion { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public string puesto { get; set; }
        public int estatus { get; set; }
        public string nombreAutorizante { get; set; }

      
    }
}
