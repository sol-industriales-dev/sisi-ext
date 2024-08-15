using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_BitacoraActividadesMantProy
    {
        public int id { get; set; }
        public int idAct { get; set; }
        public int idMant { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioCap { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool estatus { get; set; }
        public int idPm { get; set; }
        public bool aplicar { get; set; }
    }
}
