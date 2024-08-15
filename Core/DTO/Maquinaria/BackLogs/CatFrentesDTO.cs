using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class CatFrentesDTO
    {
        public int id { get; set; }
        public string nombreFrente { get; set; }
        public int idUsuarioAsignado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fecheModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
