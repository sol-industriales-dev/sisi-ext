using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Plantilla_Puesto
    {
        public int id { get; set; }
        public int id_plantilla { get; set; }
        public int puesto { get; set; }
        public int cantidad { get; set; }
        public string observaciones { get; set; }
        public bool check_bobo { get; set; }
        public bool check_autoriza { get; set; }
        public string estatus { get; set; }
        public int altas { get; set; }
        public int bajas { get; set; }
        public string cc { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }

        [ForeignKey("id_plantilla")]
        public virtual tblRH_EK_Plantilla_Personal virtualPlantilla { get; set; }

        [ForeignKey("puesto")]
        public virtual tblRH_EK_Puestos virtualPuesto { get; set; }
    }
}
