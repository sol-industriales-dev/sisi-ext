using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Plantilla_Personal
    {
        [Key]
        public int id_plantilla { get; set; }
        public string cc { get; set; }
        public int solicita { get; set; }
        public int autoriza { get; set; }
        public int vistobueno { get; set; }
        public string observaciones { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public string estatus { get; set; }
        public bool check_vobo { get; set; }
        public bool check_autoriza { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
