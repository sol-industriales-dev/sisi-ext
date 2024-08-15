using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.BajasPersonal
{
    public class tblRH_Baja_Finiquitos
    {
        public int id { get; set; }
        public int idBaja { get; set; }
        public decimal monto { get; set; }
        public string rutaFiniquito { get; set; }
        public int tipoFiniquito { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
