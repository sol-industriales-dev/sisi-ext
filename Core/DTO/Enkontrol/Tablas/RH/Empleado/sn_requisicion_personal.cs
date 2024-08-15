using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Tablas.RH.Empleado
{
    public class sn_requisicion_personal
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public int cantidad_solicitada { get; set; }
        public int solicitante { get; set; }
        public string solicitanteDesc { get; set; }
        public DateTime? fecha_solicitante { get; set; }
        public int autoriza { get; set; }
        public string autorizaDesc { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public int? visto_bueno { get; set; }
        public DateTime? fecha_visto_bueno { get; set; }
        public string estatus { get; set; }
        public decimal? sueldo_base { get; set; }
        public int jefe_inmediato { get; set; }
        public string jefe_inmediatoDesc { get; set; }
        public int? razon_solicitud { get; set; }
        public int? sustituye { get; set; }
        public DateTime? fecha_baja { get; set; }
        public DateTime fecha_contratacion { get; set; }
        public string fecha_contratacionString { get; set; }
        public string justificacion { get; set; }
        public int tipo_contrato { get; set; }
        public int? edad { get; set; }
        public string sexo { get; set; }
        public int? estado_civil { get; set; }
        public string presentacion { get; set; }
        public int? escolaridad { get; set; }
        public string especialidad { get; set; }
        public int? experiencia { get; set; }
        public string horario { get; set; }
        public string habilidades { get; set; }
        public string actividades { get; set; }
        public string conocimientos { get; set; }
        public int altas { get; set; }
        public int bajas { get; set; }
        public int id_plantilla { get; set; }
        public DateTime fecha_vigencia { get; set; }
        public string fecha_vigenciaString { get; set; }
        public string comentarioRechazo { get; set; }
        public int? idTabuladorDet { get; set; }

    }
}
