using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class RepAltasDTO
    {
        public string cC { get; set; }
        public string cCdes { get; set; }
        public string empleadoID { get; set; }
        public string empleado { get; set; }
        public string puesto { get; set; }
        public string tipo_nomina { get; set; }
        public string nss { get; set; }
        public string jefeInmediato { get; set; }
        public DateTime fechaAlta { get; set; }
        public string fechaAltaStr { get; set; }
        public string antiguedad { get; set; }
        public string fechaBaja { get; set; }
        public int clave_reg_pat { get; set; }
        public string nombre_corto { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_zona { get; set; }
        public decimal total_mensual { get; set; }
        public string EMP_ULTIMO_REINGRESO { get; set; }
        public DateTime? fecha_reingreso { get; set; }
        public DateTime? fecha_antiguedad { get; set; }
        public string ccRecon { get; set; }
        public string ccCambio { get; set; }
        public DateTime fechaCambio { get; set; }
        public string estatus_empleado { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime bajaIngreso { get; set; } //FECHA DE ANTIGUEDAD EN TABLA DE BAJAS
        public string categoria { get; set; }
    }
}
