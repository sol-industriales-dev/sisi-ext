using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Tabuladores
    {
        public int id { get; set; }
        public int idEK { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public int id_puesto { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_de_zona { get; set; }
        public decimal bono_trab_especiales { get; set; }
        public decimal bono_por_produccion { get; set; }
        public decimal bono_otros { get; set; }
        public decimal hora_extra { get; set; }
        public string observaciones { get; set; }
        public decimal nomina { get; set; }
        public bool libre { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
    }
}
