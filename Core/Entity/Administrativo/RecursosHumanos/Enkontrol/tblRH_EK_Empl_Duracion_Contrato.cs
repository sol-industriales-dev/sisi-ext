using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empl_Duracion_Contrato
    {
        public int clave_duracion { get; set; }
        public string nombre { get; set; }
        public int? duracion_meses { get; set; }
        public int? duracion_dias { get; set; }
        public string indefinido { get; set; }
        public int? tipo_contrato{ get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
