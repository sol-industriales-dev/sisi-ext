using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class CompaniaDTO
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public decimal requisicion { get; set; }
        public decimal id_regpat { get; set; }
        public string cc_contable { get; set; }
        public int puesto { get; set; }
        public int? idTabuladorDet { get; set; }
        public decimal duracion_contrato { get; set; }
        public DateTime? fecha_fin { get; set; } // PERU
        public decimal jefe_inmediato { get; set; }
        public decimal autoriza { get; set; }
        public decimal usuario_compras { get; set; }
        public string sindicato { get; set; }
        public bool chkSindicato { get; set; }
        public int clave_depto { get; set; }
        public string nss { get; set; }
        public decimal unidad_medica { get; set; }
        public string tipo_formula_imss { get; set; }
        public DateTime fecha_contrato { get; set; }
        public string actividades { get; set; }
        public int tipoEmpleado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}