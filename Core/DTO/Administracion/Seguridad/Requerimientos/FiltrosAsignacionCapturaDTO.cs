using Core.Enum.Administracion.Seguridad.Requerimientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Requerimientos
{
    public class FiltrosAsignacionCapturaDTO
    {
        public string cc { get; set; }
        public ClasificacionEnum clasificacion { get; set; }
        public int requerimientoID { get; set; }
        public int estatus { get; set; }
        public int responsable { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}
