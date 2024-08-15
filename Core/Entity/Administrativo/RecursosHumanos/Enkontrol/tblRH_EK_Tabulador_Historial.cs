using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Tabulador_Historial
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public int? tabulador { get; set; }
        public int tabulador_anterior { get; set; }
        public int FK_Tabulador { get; set; }
        public int FK_TabuladorDet { get; set; }
        public DateTime fecha_cambio { get; set; }
        public DateTime? fechaAplicaCambio { get; set; }
        public TimeSpan hora { get; set; }
        public decimal suma { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_zona { get; set; }
        public int? motivoCambio { get; set; }
        public int? FK_UsuarioCreacion { get; set; }
        public int? FK_UsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
