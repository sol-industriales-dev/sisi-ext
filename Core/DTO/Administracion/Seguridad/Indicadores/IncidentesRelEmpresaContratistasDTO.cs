using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class IncidentesRelEmpresaContratistasDTO
    {
        public int id { get; set; }
        public int idEmpresa { get; set; }
        public int idContratista { get; set; }
        public bool esActivo { get; set; }
        public int fechaCreacion { get; set; }
        public int fechaModificacion { get; set; }
        public string nomEmpresa { get; set; }
        public string nomContratista { get; set; }
    }
}
